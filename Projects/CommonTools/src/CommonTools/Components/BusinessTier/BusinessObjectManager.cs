using System;
using CommonTools.Components.Caching;
using CommonTools.Extensions;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Web;
using System.Web.Caching;
using System.Collections.Generic;
using CommonTools.Components.BusinessTier;
using System.Collections;

namespace CommonTools.Components.BusinessTier
{
    /// <summary>
    /// 
    /// </summary>
    public static class BusinessObjectManager
    {
        private static Dictionary<string, ObjectPropertyInfoContainer> _ObjectPropertyInfoContainerLookup = new Dictionary<string, ObjectPropertyInfoContainer>();
        private static readonly object _ObjectPropertyInfoContainerLookupLock = new object();

        #region Validation

        /// <summary>
        /// This method validates a given IBusinessObject against a defined database action (create/update/delete). The validation process
        /// considers the following property attributes when during execution: BusinessObjectPropertyAttribute, BusinessObjectStringSecurityAttribute
        /// and BusinessObjectValidationAttribute. Examine these attributes for further details on how to enforce validation constraints used
        /// by this class.
        /// </summary>
        /// <param name="obj">The object to validate.</param>
        /// <returns>A BusinessObjectValidationResult object which has all necessary information about the validity of the defined action.</returns>
        public static BusinessObjectValidationResult Validate(object obj)
        {
            return Validate(null, obj);
        }

        private static BusinessObjectValidationResult Validate(BusinessObjectValidationResult result, object obj)
        {
            if (obj == null)
                return new BusinessObjectValidationResult(ValidationStatus.NullReference);

            if (result == null)
                result = new BusinessObjectValidationResult(ValidationStatus.Valid);

            // get the properties of this class
            Type type = obj.GetType();
            string fullName = type.FullName;
            ObjectPropertyInfoContainer objectPropertyInfoContainer;
            if (_ObjectPropertyInfoContainerLookup.ContainsKey(fullName))
                objectPropertyInfoContainer = _ObjectPropertyInfoContainerLookup[fullName];
            else
            {
                lock (_ObjectPropertyInfoContainerLookupLock)
                {
                    if (_ObjectPropertyInfoContainerLookup.ContainsKey(fullName))
                        objectPropertyInfoContainer = _ObjectPropertyInfoContainerLookup[fullName];
                    else
                    {
                        objectPropertyInfoContainer = new ObjectPropertyInfoContainer(type);
                        _ObjectPropertyInfoContainerLookup.Add(fullName, objectPropertyInfoContainer);
                    }
                }
            }

            // this string will hold the error message of the exception thrown when a mandatory field
            // constraint was broken

            ObjectPropertyInfo objectPropertyInfo;
            object value;
            for (int i = 0; i < objectPropertyInfoContainer.Length; i++)
            {
                objectPropertyInfo = objectPropertyInfoContainer.ObjectPropertyInfos[i];

                if (objectPropertyInfo.HasBusinessObjectPropertyAttribute)
                {
                    object o = null;
                    try
                    {
                        // try to get the value of this property. This method will throw a TargetInvocationException if no 
                        // value was provided.
                        o = objectPropertyInfo.PropertyInfo.GetValue(obj, null);
                    }
                    catch (TargetInvocationException) { }
                    if (o == null
                        || (o is string && string.IsNullOrEmpty((string)o)))
                    {
                        if (objectPropertyInfo.BusinessObjectPropertyAttribute.IsMandatoryForInstance)
                        {
                            string errorMessage = string.Empty;
                            string errorMessageResourceKey = string.Empty;

                            if (objectPropertyInfo.HasBusinessObjectValidationAttribute)
                            {
                                errorMessage = objectPropertyInfo.BusinessObjectValidationAttribute.IsRequiredMessage;
                                errorMessageResourceKey = objectPropertyInfo.BusinessObjectValidationAttribute.IsRequiredMessageResourceKey;
                            }

                            result.MandatoryFieldViolations.Add(new MandatoryFieldViolation(objectPropertyInfo.PropertyInfo.Name, errorMessage, errorMessageResourceKey));

                            result.ValidationStatus = ValidationStatus.NotAllMandatoryFieldsProvided;

                        }
                        else
                        {// this property can be null/empty
                            // if there is a default value defined, assign it to this property
                            if (objectPropertyInfo.BusinessObjectPropertyAttribute.DefaultValue != null)
                                objectPropertyInfo.PropertyInfo.SetValue(obj, objectPropertyInfo.BusinessObjectPropertyAttribute.DefaultValue, null);
                        }
                    }


                    value = null;
                    try
                    {
                        // try to get the value of this property. This method will throw a TargetInvocationException if no 
                        // value was provided.
                        value = objectPropertyInfo.PropertyInfo.GetValue(obj, null);
                    }
                    catch (TargetInvocationException) { }
                    if (value == null
                        || (value is string && string.IsNullOrEmpty((string)value)))
                    {
                        // no value provided, so continue...
                        continue;
                    }

                    if (objectPropertyInfo.BusinessObjectPropertyAttribute.PropagateValidation)
                    {// alright, we now want to fully validate this property as well
                        if (value is IEnumerable)
                        {
                            foreach (object record in (IEnumerable)value)
                                result = Validate(result, record);
                        }
                        else
                            result = Validate(result, value);
                    }
                    else
                    {
                        switch (objectPropertyInfo.PropertyType)
                        {
                            case ObjectPropertyInfo.PropertyTypeDefinition.String:
                                #region String
                                string s = value as System.String;
                                if (s != null)
                                {// object is string
                                    // check for replacements

                                    if (objectPropertyInfo.HasBusinessObjectStringSecurityAttribute)
                                    {
                                        if (objectPropertyInfo.BusinessObjectStringSecurityAttribute.RemoveBadHtmlTags)
                                        {
                                            s = s.RemoveMaliciousTags();
                                        }
                                        if (objectPropertyInfo.BusinessObjectStringSecurityAttribute.RemoveScriptTags)
                                        {
                                            s = s.RemoveScriptTags();
                                        }
                                        if (objectPropertyInfo.BusinessObjectStringSecurityAttribute.RemoveBadSQLCharacters)
                                        {
                                            s = s.RemoveMaliciousSQLCharacters();
                                        }
                                        if (objectPropertyInfo.BusinessObjectStringSecurityAttribute.DefuseScriptTags)
                                        {
                                            s = s.DefuseScriptTags();
                                        }
                                    }
                                    else
                                    {
                                        s = s.DefuseScriptTags();
                                    }

                                    objectPropertyInfo.PropertyInfo.SetValue(obj, s, null);

                                    if (objectPropertyInfo.BusinessObjectValidationAttribute != null)
                                    {
                                        if (s.Length > objectPropertyInfo.BusinessObjectValidationAttribute.MaxLength && objectPropertyInfo.BusinessObjectValidationAttribute.MaxLength >= 0)
                                        {
                                            result.PropertyErrors.Add(new PropertyError(
                                                PropertyValidationError.MaximumValueConstraint
                                                , objectPropertyInfo.PropertyInfo.Name
                                                , objectPropertyInfo.BusinessObjectValidationAttribute.OutOfRangeErrorMessage
                                                , objectPropertyInfo.BusinessObjectValidationAttribute.OutOfRangeErrorMessageResourceKey));
                                        }
                                        if (s.Length < objectPropertyInfo.BusinessObjectValidationAttribute.MinLength && objectPropertyInfo.BusinessObjectValidationAttribute.MinLength >= 0)
                                        {
                                            result.PropertyErrors.Add(new PropertyError(
                                                PropertyValidationError.MinimumValueConstraint
                                                , objectPropertyInfo.PropertyInfo.Name
                                                , objectPropertyInfo.BusinessObjectValidationAttribute.OutOfRangeErrorMessage
                                                , objectPropertyInfo.BusinessObjectValidationAttribute.OutOfRangeErrorMessageResourceKey));
                                        }
                                        if (!string.IsNullOrEmpty(objectPropertyInfo.BusinessObjectValidationAttribute.Regex))
                                        {
                                            Regex regex = new Regex(objectPropertyInfo.BusinessObjectValidationAttribute.Regex);

                                            if (!regex.IsMatch(s))
                                            {
                                                result.PropertyErrors.Add(new PropertyError(
                                                    PropertyValidationError.RegexMatchConstraint
                                                    , objectPropertyInfo.PropertyInfo.Name
                                                    , objectPropertyInfo.BusinessObjectValidationAttribute.RegexMessage
                                                    , objectPropertyInfo.BusinessObjectValidationAttribute.RegexMessageResourceKey));
                                            }
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case ObjectPropertyInfo.PropertyTypeDefinition.Decimal:
                                #region Decimal
                                if (objectPropertyInfo.BusinessObjectValidationAttribute != null
                                    && !string.IsNullOrEmpty(objectPropertyInfo.BusinessObjectValidationAttribute.MinimumValue) && !string.IsNullOrEmpty(objectPropertyInfo.BusinessObjectValidationAttribute.MaximumValue))
                                {
                                    decimal a = (decimal)value;
                                    decimal maxValue = decimal.Parse(objectPropertyInfo.BusinessObjectValidationAttribute.MaximumValue.Replace(',', '.'));
                                    decimal minValue = decimal.Parse(objectPropertyInfo.BusinessObjectValidationAttribute.MinimumValue.Replace(',', '.'));
                                    if (a > maxValue)
                                    {
                                        result.PropertyErrors.Add(new PropertyError(
                                            PropertyValidationError.MaximumValueConstraint
                                            , objectPropertyInfo.PropertyInfo.Name
                                            , objectPropertyInfo.BusinessObjectValidationAttribute.OutOfRangeErrorMessage
                                            , objectPropertyInfo.BusinessObjectValidationAttribute.OutOfRangeErrorMessageResourceKey));
                                    }
                                    if (a < minValue)
                                    {
                                        result.PropertyErrors.Add(new PropertyError(
                                             PropertyValidationError.MinimumValueConstraint
                                             , objectPropertyInfo.PropertyInfo.Name
                                             , objectPropertyInfo.BusinessObjectValidationAttribute.OutOfRangeErrorMessage
                                             , objectPropertyInfo.BusinessObjectValidationAttribute.OutOfRangeErrorMessageResourceKey));
                                    }
                                }
                                #endregion
                                break;
                            case ObjectPropertyInfo.PropertyTypeDefinition.Double:
                                #region Double
                                if (objectPropertyInfo.BusinessObjectValidationAttribute != null
                                    && !string.IsNullOrEmpty(objectPropertyInfo.BusinessObjectValidationAttribute.MinimumValue) && !string.IsNullOrEmpty(objectPropertyInfo.BusinessObjectValidationAttribute.MaximumValue))
                                {
                                    double a = (double)value;
                                    double maxValue = double.Parse(objectPropertyInfo.BusinessObjectValidationAttribute.MaximumValue.Replace(',', '.'));
                                    double minValue = double.Parse(objectPropertyInfo.BusinessObjectValidationAttribute.MinimumValue.Replace(',', '.'));
                                    if (a > maxValue)
                                    {
                                        result.PropertyErrors.Add(new PropertyError(
                                            PropertyValidationError.MaximumValueConstraint
                                            , objectPropertyInfo.PropertyInfo.Name
                                            , objectPropertyInfo.BusinessObjectValidationAttribute.OutOfRangeErrorMessage
                                            , objectPropertyInfo.BusinessObjectValidationAttribute.OutOfRangeErrorMessageResourceKey));
                                    }
                                    if (a < minValue)
                                    {
                                        result.PropertyErrors.Add(new PropertyError(
                                             PropertyValidationError.MinimumValueConstraint
                                             , objectPropertyInfo.PropertyInfo.Name
                                             , objectPropertyInfo.BusinessObjectValidationAttribute.OutOfRangeErrorMessage
                                             , objectPropertyInfo.BusinessObjectValidationAttribute.OutOfRangeErrorMessageResourceKey));
                                    }
                                }
                                #endregion
                                break;
                            case ObjectPropertyInfo.PropertyTypeDefinition.Int64:
                                #region Int64
                                if (objectPropertyInfo.BusinessObjectValidationAttribute != null
                                    && !string.IsNullOrEmpty(objectPropertyInfo.BusinessObjectValidationAttribute.MinimumValue) && !string.IsNullOrEmpty(objectPropertyInfo.BusinessObjectValidationAttribute.MaximumValue))
                                {
                                    long a = (long)value;
                                    long maxValue = long.Parse(objectPropertyInfo.BusinessObjectValidationAttribute.MaximumValue);
                                    long minValue = long.Parse(objectPropertyInfo.BusinessObjectValidationAttribute.MinimumValue);
                                    if (a > maxValue)
                                    {
                                        result.PropertyErrors.Add(new PropertyError(
                                            PropertyValidationError.MaximumValueConstraint
                                            , objectPropertyInfo.PropertyInfo.Name
                                            , objectPropertyInfo.BusinessObjectValidationAttribute.OutOfRangeErrorMessage
                                            , objectPropertyInfo.BusinessObjectValidationAttribute.OutOfRangeErrorMessageResourceKey));
                                    }
                                    if (a < minValue)
                                    {
                                        result.PropertyErrors.Add(new PropertyError(
                                             PropertyValidationError.MinimumValueConstraint
                                             , objectPropertyInfo.PropertyInfo.Name
                                             , objectPropertyInfo.BusinessObjectValidationAttribute.OutOfRangeErrorMessage
                                             , objectPropertyInfo.BusinessObjectValidationAttribute.OutOfRangeErrorMessageResourceKey));
                                    }
                                }
                                #endregion
                                break;
                            case ObjectPropertyInfo.PropertyTypeDefinition.Int32:
                                #region Int32
                                if (objectPropertyInfo.BusinessObjectValidationAttribute != null
                                    && !string.IsNullOrEmpty(objectPropertyInfo.BusinessObjectValidationAttribute.MinimumValue) && !string.IsNullOrEmpty(objectPropertyInfo.BusinessObjectValidationAttribute.MaximumValue))
                                {
                                    int a = (int)value;
                                    int maxValue = int.Parse(objectPropertyInfo.BusinessObjectValidationAttribute.MaximumValue);
                                    int minValue = int.Parse(objectPropertyInfo.BusinessObjectValidationAttribute.MinimumValue);
                                    if (a > maxValue)
                                    {
                                        result.PropertyErrors.Add(new PropertyError(
                                            PropertyValidationError.MaximumValueConstraint
                                            , objectPropertyInfo.PropertyInfo.Name
                                            , objectPropertyInfo.BusinessObjectValidationAttribute.OutOfRangeErrorMessage
                                            , objectPropertyInfo.BusinessObjectValidationAttribute.OutOfRangeErrorMessageResourceKey));
                                    }
                                    if (a < minValue)
                                    {
                                        result.PropertyErrors.Add(new PropertyError(
                                            PropertyValidationError.MinimumValueConstraint
                                            , objectPropertyInfo.PropertyInfo.Name
                                            , objectPropertyInfo.BusinessObjectValidationAttribute.OutOfRangeErrorMessage
                                            , objectPropertyInfo.BusinessObjectValidationAttribute.OutOfRangeErrorMessageResourceKey));
                                    }
                                }
                                #endregion
                                break;
                            case ObjectPropertyInfo.PropertyTypeDefinition.Int16:
                                #region Int16
                                if (!string.IsNullOrEmpty(objectPropertyInfo.BusinessObjectValidationAttribute.MinimumValue) && !string.IsNullOrEmpty(objectPropertyInfo.BusinessObjectValidationAttribute.MaximumValue))
                                {
                                    short a = (short)value;
                                    short maxValue = short.Parse(objectPropertyInfo.BusinessObjectValidationAttribute.MaximumValue);
                                    short minValue = short.Parse(objectPropertyInfo.BusinessObjectValidationAttribute.MinimumValue);
                                    if (a > maxValue)
                                    {
                                        result.PropertyErrors.Add(new PropertyError(
                                            PropertyValidationError.MaximumValueConstraint
                                            , objectPropertyInfo.PropertyInfo.Name
                                            , objectPropertyInfo.BusinessObjectValidationAttribute.OutOfRangeErrorMessage
                                            , objectPropertyInfo.BusinessObjectValidationAttribute.OutOfRangeErrorMessageResourceKey));
                                    }
                                    if (a < minValue)
                                    {
                                        result.PropertyErrors.Add(new PropertyError(
                                            PropertyValidationError.MinimumValueConstraint
                                            , objectPropertyInfo.PropertyInfo.Name
                                            , objectPropertyInfo.BusinessObjectValidationAttribute.OutOfRangeErrorMessage
                                            , objectPropertyInfo.BusinessObjectValidationAttribute.OutOfRangeErrorMessageResourceKey));
                                    }
                                }
                                #endregion
                                break;
                        }
                    }
                }
            }
            if (result.HasPropertyErrors)
            {
                result.ValidationStatus = ValidationStatus.NotAllPropertiesValid;
            }

            return result;
        }
        #endregion
    }
}
