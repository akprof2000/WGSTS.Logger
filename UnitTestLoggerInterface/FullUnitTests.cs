using Microsoft.VisualStudio.TestTools.UnitTesting;
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
