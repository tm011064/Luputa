using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Reflection;
using CommonTools.Components.BusinessTier;
using CommonTools.Components.TextResources;

namespace CommonTools.Web.UI
{
    /// <summary>
    /// This class renders all validators needed to perform sufficiant client/serverside validation for
    /// a specified BaseBusinessObject property.
    /// </summary>
    [ToolboxData("<{0}:BaseBusinessObjectValidationControl runat=server></{0}:BaseBusinessObjectValidationControl>")]
    public class BusinessObjectValidationControl : BaseValidator
    {
        #region how to use
        /*  
           <Prefix:BaseBusinessObjectValidationControl ID="myControl" runat="server" 
                ControlToValidate="txtMyTextBox"                                       
                Type="GoBusker.BLL.MusicCategory, GoBusker.BLL"
                Property="MusicCategoryID" 
                ValidationGroup="myValidationGroup" 
                ValidCssClass="MyCssClassFor_txtMyTextBox_IfInputIsValid"
                ErrorCssClass="MyCssClassFor_txtMyTextBox_IfInputIsNotValid" 
                RangeValidationDataType="Integer"
                ResourceManagerType="GoBusker.BLL.CommonResourceManager" /> 
        */
        #endregion

        #region Global Variables
        private RegularExpressionValidator regularExpressionValidator;
        private RequiredFieldValidator requiredFieldValidator;
        private RangeValidator rangeValidator;
        private CompareValidator compareValidator;
        private CustomValidator customStringLengthValidator;
        private BusinessObjectValidationAttribute _ValidatorAttribute;
        #endregion

        #region constant values
        private const string REGULAR_EXPRESSION_VALIDATOR_SUFFIX = "_rev";
        private const string CUSTOM_STRING_LENGTH_VALIDATOR_SUFFIX = "_cslv";
        private const string RANGE_VALIDATOR_SUFFIX = "_rv";
        private const string REQUIRED_FIELD_VALIDATOR_SUFFIX = "_rfv";
        private const string COMPARE_VALIDATOR_SUFFIX = "_cv";
        #endregion

        #region events
        /// <summary>
        /// Handles the ServerValidate event of the customStringLengthValidator control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.ServerValidateEventArgs"/> instance containing the event data.</param>
        protected virtual void customStringLengthValidator_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            e.IsValid = true;
            if (e.Value.Length > _ValidatorAttribute.MaxLength || e.Value.Length < _ValidatorAttribute.MinLength)
                e.IsValid = false;
        }
        #endregion

        #region Properties
        private string _ITextResourceManagerType;
        /// <summary>
        /// Gets or sets the type of the resource manager.
        /// </summary>
        /// <value>The type of the resource manager.</value>
        public string ITextResourceManagerType
        {
            get { return _ITextResourceManagerType; }
            set { _ITextResourceManagerType = value; }
        }

        private string _Type;
        /// <summary>
        /// Gets or sets the type of the BaseBusinessObject in the following format:
        /// 'MyNamespace.SubNamspace.MyObjectDerivedFromBaseBusinessObject, AssemblyName'
        /// </summary>
        /// <value>The type.</value>
        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }

        private string _Property;
        /// <summary>
        /// Gets or sets the property name of the via Type specified object.
        /// </summary>
        /// <value>The property.</value>
        public string Property
        {
            get { return _Property; }
            set { _Property = value; }
        }

        private string _ControlToCompare = null;
        /// <summary>
        /// Gets or sets the control to compare this control with if comparing is necessary.
        /// </summary>
        /// <value>The control to compare.</value>
        public string ControlToCompare
        {
            get { return _ControlToCompare; }
            set { _ControlToCompare = value; }
        }

        private string _ErrorCssClass = null;
        /// <summary>
        /// The css class of the control to validate if the validation failed. Use this property if you want to 
        /// highlight or change the backgroundcolor of an input field that was invalid.
        /// </summary>
        /// <value>The error CSS class.</value>
        public string ErrorCssClass
        {
            get { return _ErrorCssClass; }
            set { _ErrorCssClass = value; }
        }

        private string _ValidCssClass = null;
        /// <summary>
        /// The css class of the control to validate if the validation was correct ( = normal css class).
        /// </summary>
        /// <value>The valid CSS class.</value>
        public string ValidCssClass
        {
            get { return _ValidCssClass; }
            set { _ValidCssClass = value; }
        }

        private ValidationDataType _RangeValidationDataType = ValidationDataType.Integer;
        /// <summary>
        /// Gets or sets the data type that the values being compared are converted to
        /// before the comparison is made.
        /// </summary>
        /// <value>The type of the range validation data.</value>
        public ValidationDataType RangeValidationDataType
        {
            get { return _RangeValidationDataType; }
            set { _RangeValidationDataType = value; }
        }
        #endregion

        #region life-cycle
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"></see> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            Type t = System.Type.GetType(this.Type, true);
            PropertyInfo info = t.GetProperty(_Property);
            object[] attributes = (object[])info.GetCustomAttributes(typeof(BusinessObjectValidationAttribute), false);

            ITextResourceManager resourceManager = null;
            if (!string.IsNullOrEmpty(this.ITextResourceManagerType))
            {
                resourceManager = Activator.CreateInstance(System.Type.GetType(this.ITextResourceManagerType)) as ITextResourceManager;
            }

            // check all attributes
            if (attributes.Length > 0)
            {// at least on attribute exists
                _ValidatorAttribute = attributes[0] as BusinessObjectValidationAttribute;

                if (!string.IsNullOrEmpty(_ValidatorAttribute.Regex))
                {
                    regularExpressionValidator = new RegularExpressionValidator();
                    regularExpressionValidator.ValidationExpression = _ValidatorAttribute.Regex;
                    regularExpressionValidator.ErrorMessage = (resourceManager != null
                                                               && !string.IsNullOrEmpty(_ValidatorAttribute.RegexMessageResourceKey))
                                                                    ? resourceManager.GetResourceText(_ValidatorAttribute.RegexMessageResourceKey)
                                                                    : _ValidatorAttribute.RegexMessage;
                    regularExpressionValidator.ValidationGroup = this.ValidationGroup;
                    regularExpressionValidator.ControlToValidate = this.ControlToValidate;
                    regularExpressionValidator.Display = this.Display;
                    regularExpressionValidator.ID = this.ID + REGULAR_EXPRESSION_VALIDATOR_SUFFIX;
                    regularExpressionValidator.EnableClientScript = this.EnableClientScript;
                    this.Controls.Add(regularExpressionValidator);
                }

                if (_ValidatorAttribute.MaxLength >= 0 || _ValidatorAttribute.MinLength >= 0)
                {
                    customStringLengthValidator = new CustomValidator();
                    customStringLengthValidator.ErrorMessage = (resourceManager != null
                                                               && !string.IsNullOrEmpty(_ValidatorAttribute.OutOfRangeErrorMessageResourceKey))
                                                                    ? resourceManager.GetResourceText(_ValidatorAttribute.OutOfRangeErrorMessageResourceKey)
                                                                    : _ValidatorAttribute.OutOfRangeErrorMessage;
                    customStringLengthValidator.ValidationGroup = this.ValidationGroup;
                    customStringLengthValidator.ControlToValidate = this.ControlToValidate;
                    customStringLengthValidator.Display = this.Display;
                    customStringLengthValidator.ID = this.ID + CUSTOM_STRING_LENGTH_VALIDATOR_SUFFIX;
                    customStringLengthValidator.EnableClientScript = this.EnableClientScript;
                    customStringLengthValidator.ValidateEmptyText = true;
                    customStringLengthValidator.ServerValidate += new ServerValidateEventHandler(customStringLengthValidator_ServerValidate);
                    this.Controls.Add(customStringLengthValidator);
                }

                if (!string.IsNullOrEmpty(_ValidatorAttribute.MinimumValue) || !string.IsNullOrEmpty(_ValidatorAttribute.MaximumValue))
                {
                    rangeValidator = new RangeValidator();
                    rangeValidator.MinimumValue = _ValidatorAttribute.MinimumValue;
                    rangeValidator.MaximumValue = _ValidatorAttribute.MaximumValue;
                    rangeValidator.ErrorMessage = (resourceManager != null
                                                               && !string.IsNullOrEmpty(_ValidatorAttribute.OutOfRangeErrorMessageResourceKey))
                                                                    ? resourceManager.GetResourceText(_ValidatorAttribute.OutOfRangeErrorMessageResourceKey)
                                                                    : _ValidatorAttribute.OutOfRangeErrorMessage;
                    rangeValidator.ControlToValidate = this.ControlToValidate;
                    rangeValidator.Display = this.Display;
                    rangeValidator.ID = this.ID + RANGE_VALIDATOR_SUFFIX;
                    rangeValidator.EnableClientScript = this.EnableClientScript;
                    rangeValidator.Type = this.RangeValidationDataType;
                    this.Controls.Add(rangeValidator);
                }

                if (_ValidatorAttribute.IsRequired)
                {
                    requiredFieldValidator = new RequiredFieldValidator();
                    requiredFieldValidator.ControlToValidate = this.ControlToValidate;
                    requiredFieldValidator.Display = this.Display;
                    requiredFieldValidator.ErrorMessage = (resourceManager != null
                                                               && !string.IsNullOrEmpty(_ValidatorAttribute.IsRequiredMessageResourceKey))
                                                                    ? resourceManager.GetResourceText(_ValidatorAttribute.IsRequiredMessageResourceKey)
                                                                    : _ValidatorAttribute.IsRequiredMessage;
                    requiredFieldValidator.ID = this.ID + REQUIRED_FIELD_VALIDATOR_SUFFIX;
                    requiredFieldValidator.EnableClientScript = this.EnableClientScript;
                    this.Controls.Add(requiredFieldValidator);
                }

                if (!string.IsNullOrEmpty(this.ControlToCompare))
                {
                    compareValidator = new CompareValidator();
                    compareValidator.ControlToCompare = this.ControlToCompare;
                    compareValidator.ControlToValidate = this.ControlToValidate;
                    compareValidator.Display = this.Display;
                    compareValidator.ID = this.ID + COMPARE_VALIDATOR_SUFFIX;
                    compareValidator.EnableClientScript = this.EnableClientScript;
                    this.Controls.Add(compareValidator);
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"></see> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            if (customStringLengthValidator != null)
            {// custom string length validation is necessary for this control
                string functionName = this.ID + "IsLengthValid";
                customStringLengthValidator.ClientValidationFunction = functionName;
                Control controlToValidate = FindControl(this.ControlToValidate);
                this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), functionName,
@"
function " + functionName + @"(source, argument) {
    var element = document.getElementById('" + controlToValidate.ClientID + @"');
    argument.IsValid = true;
    if (element != null) {
        if (element.value.length > " + _ValidatorAttribute.MaxLength + @" || element.value.length <" + _ValidatorAttribute.MinLength + @")
            argument.IsValid = false;
    }
}
", true);
                customStringLengthValidator.ClientValidationFunction = functionName;
            }

            if (this.EnableClientScript)
            {
                // register client script toggle css function
                if (!this.Page.ClientScript.IsClientScriptBlockRegistered("toggleCssClassOnValidation"))
                {
                    this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "toggleCssClassOnValidation",
@"
function ToggleCssClassOnError(sender, validClass, errorClass) { 
    var isValid = true;
    for (var i = 0; i < this.Page_Validators.length; i++) { 
        if (Page_Validators[i].controltovalidate == sender.id) {
            if (!Page_Validators[i].evaluationfunction(Page_Validators[i])) {
                isValid = false; 
            } 
        }
    }
    sender.className = (isValid ? validClass : errorClass);             
}
", true);
                }

                // add the client side toggle css function call to the control's onblur event.
                WebControl c = FindControl(this.ControlToValidate) as WebControl;
                c.Attributes["onblur"] += "ToggleCssClassOnError(this, '" + ValidCssClass + "', '" + ErrorCssClass + "');";
            }
        }

        /// <summary>
        /// Displays the control on the client.
        /// </summary>
        /// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter"></see> that contains the output stream for rendering on the client.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (this.rangeValidator != null)
                rangeValidator.RenderControl(writer);

            if (this.requiredFieldValidator != null)
                requiredFieldValidator.RenderControl(writer);

            if (this.compareValidator != null)
                compareValidator.RenderControl(writer);

            if (this.regularExpressionValidator != null)
                regularExpressionValidator.RenderControl(writer);

            if (this.customStringLengthValidator != null)
                customStringLengthValidator.RenderControl(writer);
        }

        #endregion

        #region overrides
        /// <summary>
        /// When overridden in a derived class, this method contains the code to determine whether the value in the input control is valid.
        /// </summary>
        /// <returns>
        /// true if the value in the input control is valid; otherwise, false.
        /// </returns>
        protected override bool EvaluateIsValid()
        {
            if (this.rangeValidator != null)
            {
                rangeValidator.Validate();
                if (!this.rangeValidator.IsValid)
                    return false;
            }

            if (this.requiredFieldValidator != null)
            {
                requiredFieldValidator.Validate();
                if (!this.requiredFieldValidator.IsValid)
                    return false;
            }

            if (this.compareValidator != null)
            {
                compareValidator.Validate();
                if (!this.compareValidator.IsValid)
                    return false;
            }

            if (this.regularExpressionValidator != null)
            {
                regularExpressionValidator.Validate();
                if (!this.regularExpressionValidator.IsValid)
                    return false;
            }

            if (this.customStringLengthValidator != null)
            {
                customStringLengthValidator.Validate();
                if (!this.customStringLengthValidator.IsValid)
                    return false;
            }

            return true;
        }
        #endregion
    }
}