using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.IO;
using WGSTS.Logger;

namespace UnitTestLoggers
{
    [TestClass]
    public class FullUnitTests
    {



        [TestMethod]
        public void IntefaceFromJsonJHelp()
        {
            var str = "sdjhasdkjasdhjkas".ToJson().FromJson(typeof(string));

            str = "sdjhasdkjasdhjkas".ToJson(true).FromJson(typeof(string));

            Assert.AreEqual(str, "sdjhasdkjasdhjkas");

            str = @"{sdjhasdkjasdhjkas}".FromJson(typeof(string));

            Assert.AreEqual(str, null);

        }

        [TestMethod]
        public void IntefaceFromJsonJHelpT()
        {
            var str = @"sdjhasdkjasdhjkas".ToJson().FromJson<string>();

            Assert.AreEqual(str, "sdjhasdkjasdhjkas");

            str = @"{sdjhasdkjasdhjkas}".FromJson<string>();

            Assert.AreEqual(str, null);
        }


        [TestMethod]
        public void IntefaceToJsonJHelpT()
        {
            var str = @"sdjhasdkjasdhjkas".ToJson();

            Assert.AreEqual(str, @"""sdjhasdkjasdhjkas""");

        }


        class Data
        {
            public IEnumerable Val { get; set; }
        }


        interface ITestData
        {
            int Test { get; set; }
        }

        class Data1 : ITestData
        {
            public int Test { get; set; }

            public string TestString { get; set; }
        }


        class Data2 : ITestData
        {
            public int Test { get; set; }

            public decimal TestDecimal { get; set; }
        }



        [TestMethod]
        public void IntefaceToJsonAutoJHelpT()
        {
            var data = new Data() { Val = new byte[] { 1, 2, 3 } };

            //            File.WriteAllText(@"c:\temp\data1.json", inf.ToJsonAuto());

            Assert.AreEqual(data.ToJsonAuto(), @"{""Val"":{""$type"":""System.Byte[], System.Private.CoreLib"",""$value"":""AQID""}}");

            ITestData str = (ITestData)(new Data1() { Test = 1, TestString = "str" });

            str = (ITestData)(new Data2() { Test = 1, TestDecimal = 0.5M });

            var inf = str.ToJsonAuto().FromJsonAuto<ITestData>();
            Assert.AreEqual(null, inf);


        }


        [TestMethod]
        public void IntefaceFromJsonAutoJHelpT()
        {
            var data = new Data() { Val = new byte[] { 1, 2, 3 } };
            var str = @"{""Val"":{""$type"":""System.Byte[], System.Private.CoreLib"",""$value"":""AQID""}}".FromJsonAuto<Data>();
            CompareLogic compareLogic = new CompareLogic();
            ComparisonResult result;
            result = compareLogic.Compare(data, str);
            Assert.AreEqual(true, result.AreEqual);


            str = (Data)@"{""Val"":{""$type"":""System.Byte[], System.Private.CoreLib"",""$value"":""AQID""}}".FromJsonAuto(typeof(Data));
            compareLogic = new CompareLogic();
            
            result = compareLogic.Compare(data, str);
            Assert.AreEqual(true, result.AreEqual);
        }


        [TestMethod]
        public void IntefaceToJsonFromAllJHelpT()
        {
            ITestData str = (ITestData)(new Data1() { Test = 1, TestString = "str" });
            str = (ITestData)(new Data2() { Test = 1, TestDecimal = 0.5M });
            var inf = str.ToJsonAll().FromJsonAll<ITestData>();
            CompareLogic compareLogic = new CompareLogic();
            ComparisonResult result;
            result = compareLogic.Compare(inf, str);
            Assert.AreEqual(true, result.AreEqual);

            inf = (ITestData)str.ToJsonAll().FromJsonAll(typeof(ITestData));
            compareLogic = new CompareLogic();
            result = compareLogic.Compare(inf, str);
            Assert.AreEqual(true, result.AreEqual);
        }

        [TestMethod]
        public void IntefaceMemoryStreamToJson()
        {
            var ms = new MemoryStream();
            ms.WriteByte(149);

            var str = ms.MemoryStreamToJson();

            Assert.AreEqual(str, "\"lQ==\"");
        }

        [TestMethod]
        public void IntefaceMemoryStreamFromJson()
        {
            var json = "\"lQ==\"";
            var bytes = (byte[])json.MemoryStreamFromJson();

            Assert.AreEqual(bytes[0], 149);
        }

        [TestMethod]
        public void IntefaceTestDummyLogger()
        {
            var logger = new DummyLogger();

            var log = (ILogger)logger;

            log.Fatal("Big test dummy logger");
        }


    }
}
