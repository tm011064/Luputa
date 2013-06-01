using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using CommonTools.Web.Security;
using System.Web;
using System.Diagnostics;
using System.Text.RegularExpressions;
using CommonTools.Components.BusinessTier;
using CommonTools.Finance.RegularExpressions;

namespace CommonTools.TestSuite
{
    [TestFixture]
    public class ValidationExpressionsTests
    {
        [Test]
        public void Test_EmailRegex()
        {
            Assert.IsTrue(new Regex(CommonTools.Components.RegularExpressions.ValidationExpressions.EmailAddress).IsMatch("email@test.com"));
            Assert.IsFalse(new Regex(CommonTools.Components.RegularExpressions.ValidationExpressions.EmailAddress).IsMatch("invalidemail"));
        }

        [Test]
        public void Test_MultipleEmailsRegex()
        {
            Assert.IsTrue(CommonTools.Components.RegularExpressions.ValidationExpressions.AreValidEmails("admin@fantasyleague.com; sdfsdf", ','));
        }

        [Test]
        public void Test_IsISINValid()
        {
            Assert.IsTrue(ValidationExpressions.IsISINValid("USCV6JZH0DI0"));
            Assert.IsTrue(ValidationExpressions.IsISINValid("US0464331083"));
            Assert.IsTrue(ValidationExpressions.IsISINValid("NL0000440584"));
            Assert.IsTrue(ValidationExpressions.IsISINValid("US75061P1021"));
            Assert.IsTrue(ValidationExpressions.IsISINValid("US6102361010"));
            Assert.IsTrue(ValidationExpressions.IsISINValid("GB0001528156"));
            Assert.IsTrue(ValidationExpressions.IsISINValid("US25269L1061"));
            Assert.IsTrue(ValidationExpressions.IsISINValid("BRARCEACNOR7"));
            Assert.IsTrue(ValidationExpressions.IsISINValid("US7549071030"));
            Assert.IsTrue(ValidationExpressions.IsISINValid("US12811R1041"));
            Assert.IsTrue(ValidationExpressions.IsISINValid("CA29381P1027"));
            Assert.IsTrue(ValidationExpressions.IsISINValid("TW0002883006"));
            Assert.IsTrue(ValidationExpressions.IsISINValid("US74460D7295"));
            Assert.IsTrue(ValidationExpressions.IsISINValid("FR0004008209"));
            Assert.IsTrue(ValidationExpressions.IsISINValid("US7739031091"));
            Assert.IsTrue(ValidationExpressions.IsISINValid("ZAE000085346"));
            Assert.IsTrue(ValidationExpressions.IsISINValid("AU000000IPG1"));
            Assert.IsTrue(ValidationExpressions.IsISINValid("GB0000130756"));
            Assert.IsTrue(ValidationExpressions.IsISINValid("TW0006205008"));
            Assert.IsTrue(ValidationExpressions.IsISINValid("US74438Q1094"));
            Assert.IsTrue(ValidationExpressions.IsISINValid("FR0010037242"));
            Assert.IsTrue(ValidationExpressions.IsISINValid("US82705T1025"));
            Assert.IsTrue(ValidationExpressions.IsISINValid("NL0000888691"));
            Assert.IsTrue(ValidationExpressions.IsISINValid("US4525211078"));
            Assert.IsTrue(ValidationExpressions.IsISINValid("US8321101003"));
            Assert.IsTrue(ValidationExpressions.IsISINValid("AU000000VPG4"));
            Assert.IsTrue(ValidationExpressions.IsISINValid("US8787273049"));
            Assert.IsTrue(ValidationExpressions.IsISINValid("US17311G7714"));

            Assert.IsFalse(ValidationExpressions.IsISINValid("US0464331081"));
            Assert.IsFalse(ValidationExpressions.IsISINValid("NL0000440581"));
            Assert.IsFalse(ValidationExpressions.IsISINValid("US75061P1022"));
            Assert.IsFalse(ValidationExpressions.IsISINValid("US6102361011"));
            Assert.IsFalse(ValidationExpressions.IsISINValid("GB0001528151"));
            Assert.IsFalse(ValidationExpressions.IsISINValid("US25269L1062"));
            Assert.IsFalse(ValidationExpressions.IsISINValid("BRARCEACNOR1"));
            Assert.IsFalse(ValidationExpressions.IsISINValid("US7549071031"));
            Assert.IsFalse(ValidationExpressions.IsISINValid("US12811R1042"));
            Assert.IsFalse(ValidationExpressions.IsISINValid("CA29381P1021"));
            Assert.IsFalse(ValidationExpressions.IsISINValid("TW0002883001"));
            Assert.IsFalse(ValidationExpressions.IsISINValid("US74460D7291"));
            Assert.IsFalse(ValidationExpressions.IsISINValid("FR0004008201"));
            Assert.IsFalse(ValidationExpressions.IsISINValid("US7739031092"));
            Assert.IsFalse(ValidationExpressions.IsISINValid("ZAE000085341"));
            Assert.IsFalse(ValidationExpressions.IsISINValid("AU000000IPG2"));
            Assert.IsFalse(ValidationExpressions.IsISINValid("GB0000130751"));
            Assert.IsFalse(ValidationExpressions.IsISINValid("TW0006205002"));
            Assert.IsFalse(ValidationExpressions.IsISINValid("US74438Q1093"));
            Assert.IsFalse(ValidationExpressions.IsISINValid("FR0010037241"));
            Assert.IsFalse(ValidationExpressions.IsISINValid("US82705T1022"));
            Assert.IsFalse(ValidationExpressions.IsISINValid("NL0000888693"));
            Assert.IsFalse(ValidationExpressions.IsISINValid("US4525211071"));
            Assert.IsFalse(ValidationExpressions.IsISINValid("US8321101001"));
            Assert.IsFalse(ValidationExpressions.IsISINValid("AU000000VPG1"));
            Assert.IsFalse(ValidationExpressions.IsISINValid("US8787273041"));
            Assert.IsFalse(ValidationExpressions.IsISINValid("US17311G7711"));
        }

        [Test]
        public void Test_IsSedolValid()
        {
            Assert.IsTrue(ValidationExpressions.IsSedolValid("B0R8PP6"));
            Assert.IsTrue(ValidationExpressions.IsSedolValid("2473138"));
            Assert.IsTrue(ValidationExpressions.IsSedolValid("B02J6B7"));
            Assert.IsTrue(ValidationExpressions.IsSedolValid("2195669"));
            Assert.IsTrue(ValidationExpressions.IsSedolValid("6431756"));
            Assert.IsTrue(ValidationExpressions.IsSedolValid("2614012"));
            Assert.IsTrue(ValidationExpressions.IsSedolValid("5983173"));
            Assert.IsTrue(ValidationExpressions.IsSedolValid("2754060"));
            Assert.IsTrue(ValidationExpressions.IsSedolValid("B1G4262"));
            Assert.IsTrue(ValidationExpressions.IsSedolValid("6954145"));
            Assert.IsTrue(ValidationExpressions.IsSedolValid("0013075"));
            Assert.IsTrue(ValidationExpressions.IsSedolValid("6431972"));
            Assert.IsTrue(ValidationExpressions.IsSedolValid("2563716"));
            Assert.IsTrue(ValidationExpressions.IsSedolValid("B01TLR9"));
            Assert.IsTrue(ValidationExpressions.IsSedolValid("2497581"));
            Assert.IsTrue(ValidationExpressions.IsSedolValid("B1Z95S1"));
            Assert.IsTrue(ValidationExpressions.IsSedolValid("2517854"));
            Assert.IsTrue(ValidationExpressions.IsSedolValid("2816409"));
            Assert.IsTrue(ValidationExpressions.IsSedolValid("6570121"));
            Assert.IsTrue(ValidationExpressions.IsSedolValid("2880181"));
            Assert.IsTrue(ValidationExpressions.IsSedolValid("B1XQQR0"));

            Assert.IsFalse(ValidationExpressions.IsSedolValid("B0R8PP1"));
            Assert.IsFalse(ValidationExpressions.IsSedolValid("2473131"));
            Assert.IsFalse(ValidationExpressions.IsSedolValid("B02J6B1"));
            Assert.IsFalse(ValidationExpressions.IsSedolValid("2195661"));
            Assert.IsFalse(ValidationExpressions.IsSedolValid("6431751"));
            Assert.IsFalse(ValidationExpressions.IsSedolValid("2614011"));
            Assert.IsFalse(ValidationExpressions.IsSedolValid("5983171"));
            Assert.IsFalse(ValidationExpressions.IsSedolValid("2754061"));
            Assert.IsFalse(ValidationExpressions.IsSedolValid("B1G4261"));
            Assert.IsFalse(ValidationExpressions.IsSedolValid("6954141"));
            Assert.IsFalse(ValidationExpressions.IsSedolValid("0013071"));
            Assert.IsFalse(ValidationExpressions.IsSedolValid("6431971"));
            Assert.IsFalse(ValidationExpressions.IsSedolValid("2563711"));
            Assert.IsFalse(ValidationExpressions.IsSedolValid("B01TLR1"));
            Assert.IsFalse(ValidationExpressions.IsSedolValid("2497582"));
            Assert.IsFalse(ValidationExpressions.IsSedolValid("B1Z95S2"));
            Assert.IsFalse(ValidationExpressions.IsSedolValid("2517851"));
            Assert.IsFalse(ValidationExpressions.IsSedolValid("2816401"));
            Assert.IsFalse(ValidationExpressions.IsSedolValid("6570122"));
            Assert.IsFalse(ValidationExpressions.IsSedolValid("2880182"));
            Assert.IsFalse(ValidationExpressions.IsSedolValid("B1XQQR1"));
        }
    }
}
