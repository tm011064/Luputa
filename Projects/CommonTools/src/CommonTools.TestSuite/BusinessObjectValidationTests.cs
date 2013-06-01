using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using CommonTools.Web.Security;
using System.Web;
using System.Diagnostics;
using CommonTools.TestSuite.Components;
using System.Text.RegularExpressions;
using CommonTools.Components.BusinessTier;
using CommonTools.Components.RegularExpressions;
using CommonTools.Components.Testing;

namespace CommonTools.TestSuite
{
    [TestFixture]
    public class BusinessObjectValidationTests
    {
        #region members
        private class MyChild
        {
            [BusinessObjectProperty(IsMandatoryForInstance = false)]
            [BusinessObjectValidation(MinimumValue = "100", MaximumValue = "200")]
            public int SomeInteger { get; set; }

            public MyChild(int someInteger)
            {
                this.SomeInteger = someInteger;
            }
        }

        private class MyParent
        {
            [BusinessObjectProperty(IsMandatoryForInstance = false, PropagateValidation = true)]
            public MyChild MyChild { get; set; }

            [BusinessObjectProperty(IsMandatoryForInstance = false, PropagateValidation = true)]
            public List<MyChild> MyChildList { get; set; }
        }

        private class MyTestClass
        {
            [BusinessObjectProperty(IsMandatoryForInstance = true)]
            [BusinessObjectValidation(Regex = @"^[a-zA-Z0-9\-]{1,40}$")]
            public string Name { get; set; }

            [BusinessObjectProperty(IsMandatoryForInstance = true)]
            [BusinessObjectValidation(Regex = ValidationExpressions.EmailAddress)]
            public string Email { get; set; }

            [BusinessObjectValidation(MinimumValue = "100", MaximumValue = "200")]
            public int SomeInteger { get; set; }

            [BusinessObjectProperty(IsMandatoryForInstance = false)]
            [BusinessObjectValidation(MinimumValue = "100", MaximumValue = "200")]
            public long SomeLong { get; set; }

            [BusinessObjectProperty(IsMandatoryForInstance = true, PropagateValidation = true)]
            public MyChild MyChild { get; set; }

            [BusinessObjectProperty(IsMandatoryForInstance = true, PropagateValidation = true)]
            public List<MyChild> MyChildList { get; set; }
        }

        private enum MyStatus { Valid, Invalid }
        #endregion

        [Test]
        public void Test_BusinessObjectRegexValidation()
        {
            // TODO (Roman): Write a proper test for this

            MyTestClass testClass = new MyTestClass();

            testClass.Name = "abcDEF123--";
            testClass.Email = "hello@test.com";
            testClass.SomeInteger = 144;
            testClass.SomeLong = 155;

            Assert.IsTrue(BusinessObjectManager.Validate(testClass).IsValid);
        }

        [Test]
        public void Test_ChildElementPropagation()
        {
            int validInteger = 120;
            int invalidInteger = 1000;

            MyParent myParent = new MyParent();
            
            myParent.MyChild = new MyChild(invalidInteger);

            BusinessObjectValidationResult businessObjectValidationResult = BusinessObjectManager.Validate(myParent);
            Assert.AreEqual(ValidationStatus.NotAllPropertiesValid, businessObjectValidationResult.ValidationStatus);
            Trace.WriteLine(businessObjectValidationResult.ToString(TextFormat.ASCII));



            myParent.MyChild = new MyChild(validInteger);
            businessObjectValidationResult = BusinessObjectManager.Validate(myParent);
            Assert.AreEqual(ValidationStatus.Valid, businessObjectValidationResult.ValidationStatus);
            Trace.WriteLine(businessObjectValidationResult.ToString(TextFormat.ASCII));



            myParent.MyChildList = new List<MyChild>();
            myParent.MyChildList.Add(new MyChild(validInteger));
            myParent.MyChildList.Add(new MyChild(validInteger));
            myParent.MyChildList.Add(new MyChild(validInteger));

            businessObjectValidationResult = BusinessObjectManager.Validate(myParent);
            Assert.AreEqual(ValidationStatus.Valid, businessObjectValidationResult.ValidationStatus);
            Trace.WriteLine(businessObjectValidationResult.ToString(TextFormat.ASCII));
            


            myParent.MyChildList = new List<MyChild>();
            myParent.MyChildList.Add(new MyChild(validInteger));
            myParent.MyChildList.Add(new MyChild(invalidInteger));
            myParent.MyChildList.Add(new MyChild(invalidInteger));

            businessObjectValidationResult = BusinessObjectManager.Validate(myParent);
            Assert.AreEqual(ValidationStatus.NotAllPropertiesValid, businessObjectValidationResult.ValidationStatus);

            Trace.WriteLine(businessObjectValidationResult.ToString(TextFormat.ASCII));
        }

    }
}
