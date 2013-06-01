using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Reflection;
using System.IO;
using CommonTools.Data;
using System.Diagnostics;

namespace CommonTools.TestSuite
{
    [TestFixture]
    public class ExcelReaderTests
    {
        private void DoSimpleReadTest(string filePath)
        {
            Assert.DoesNotThrow(
                delegate
                {
                    using (ExcelReader excelReader = new ExcelReader(filePath))
                    {
                        string stringFieldValue;
                        Assert.IsTrue(excelReader.TryGetValue<string>("EMLE", "B7", out stringFieldValue));
                        Assert.AreEqual("EUR", stringFieldValue);

                        double doubleFieldValue;
                        Assert.IsTrue(excelReader.TryGetValue<double>("EMLE", "B17", out doubleFieldValue));
                        Assert.AreEqual(360390794.17, doubleFieldValue);

                        int i = 0;
                        foreach (object[] itemArray in excelReader.IterateOverWorksheetRows("EMLE", 33))
                        {
                            Trace.WriteLine((++i).ToString().PadRight(3) + "." + ConversionHelper.GetCollectionString(itemArray, "\t"));
                        }
                    }
                });
        }

        [Test]
        public void Test_2003_ReadValue()
        {
            DoSimpleReadTest(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"DataSources\TestFile_2003.xls"));
        }
        [Test]
        public void Test_2007_ReadValue()
        {
            DoSimpleReadTest(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"DataSources\TestFile_2007.xlsx"));
        }



        private void DoSimpleReadTest2(string filePath)
        {
            Assert.DoesNotThrow(
                delegate
                {
                    using (ExcelReader excelReader = new ExcelReader(filePath))
                    {
                        string stringFieldValue;
                        Assert.IsTrue(excelReader.TryGetValue<string>("IBOXX EUR LIQUID C##DIVERSIFIED", "C11", out stringFieldValue));
                        Assert.AreEqual("EUR", stringFieldValue);

                        double doubleFieldValue;
                        Assert.IsTrue(excelReader.TryGetValue<double>("IBOXX EUR LIQUID C##DIVERSIFIED", "C14", out doubleFieldValue));
                        Assert.AreEqual(3507820.58, doubleFieldValue);

                        int i = 0;
                        foreach (object[] itemArray in excelReader.IterateOverWorksheetRows("IBOXX EUR LIQUID C##DIVERSIFIED", 19))
                        {
                            Trace.WriteLine((++i).ToString().PadRight(3) + "." + ConversionHelper.GetCollectionString(itemArray, "\t"));
                        }
                    }
                });
        }
        [Test]
        public void Test_2003_ReadValue_2()
        {
            DoSimpleReadTest2(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"DataSources\TestFile2_2003.xls"));
        }
    }
}
