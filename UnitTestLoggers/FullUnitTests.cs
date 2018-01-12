using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;
using WGSTS.Logger;

namespace UnitTestLoggers
{
    [TestClass]
    public class FullUnitTests
    {
        string _dir = null;
      

        [TestMethod]
        public void FromJsonJHelp()
        {
            var str = "sdjhasdkjasdhjkas".ToJson().FromJson(typeof(string));

            str = "sdjhasdkjasdhjkas".ToJson(true).FromJson(typeof(string));

            Assert.AreEqual(str, "sdjhasdkjasdhjkas");

            str = @"{sdjhasdkjasdhjkas}".FromJson(typeof(string));

            Assert.AreEqual(str, null);

        }

        [TestMethod]
        public void FromJsonJHelpT()
        {
            var str = @"sdjhasdkjasdhjkas".ToJson().FromJson<string>();

            Assert.AreEqual(str, "sdjhasdkjasdhjkas");

            str = @"{sdjhasdkjasdhjkas}".FromJson<string>();

            Assert.AreEqual(str, null);
        }


        [TestMethod]
        public void ToJsonJHelpT()
        {
            var str = @"sdjhasdkjasdhjkas".ToJson();

            Assert.AreEqual(str, @"""sdjhasdkjasdhjkas""");

        }

        [TestMethod]
        public void MemoryStreamToJson()
        {
            var ms = new MemoryStream();
            ms.WriteByte(149);

            var str = ms.MemoryStreamToJson();

            Assert.AreEqual(str, "\"lQ==\"");
        }

        [TestMethod]
        public void MemoryStreamFromJson()
        {
            var json = "\"lQ==\"";
            var bytes = (byte[])json.MemoryStreamFromJson();

            Assert.AreEqual(bytes[0], 149);
        }

        [TestMethod]
        public void TestCountCompressionFileLog()
        {
            TestSetFileNameLogger();
            Logger.FileSize = 1;
            Logger.FileCount = 5;
            Logger.Compression = true;


            for (int i = 0; i < 5; i++)
            {
                var files = Directory.GetFiles($@"{_dir}", "*.log");
                foreach (var item in files)
                {
                    try
                    {
                        if (File.Exists(item))
                        {

                            File.SetAttributes(item, FileAttributes.Normal);
                            File.Delete(item);
                        }
                    }
                    catch
                    {
                        Thread.Sleep(50);
                    }
                }
            }



            for (int i = 0; i < 5; i++)
            {
                var files = Directory.GetFiles($@"{_dir}", "*.zip");
                foreach (var item in files)
                {
                    try
                    {
                        if (File.Exists(item))
                        {

                            File.SetAttributes(item, FileAttributes.Normal);
                            File.Delete(item);
                        }
                    }
                    catch
                    {
                        Thread.Sleep(50);
                    }
                }
            }


            try
            {
                Directory.Delete(_dir);
            }
            catch
            { }


            Logger.Error(bigtext);
            Thread.Sleep(100);
            Logger.Flush();
            for (int j = 0; j < 6; j++)
            {
                Logger.Error(bigtext);
                for (int i = 0; i < 30; i++)
                {
                    if (File.Exists($@"{_dir}\test.0{j}.zip"))
                    {
                        break;
                    }
                    Thread.Sleep(12);
                }
            }
            var files1 = Directory.GetFiles($@"{_dir}", "test*.zip");

            Assert.AreEqual(files1.Length, 5);

        }


        [TestMethod]
        public void TestSetFileNameLogger()
        {
            _dir = _dir ?? Path.Combine(Environment.CurrentDirectory, "trace");

            var fn = Path.Combine(_dir, "test.log");


            Directory.CreateDirectory(_dir);

            Logger.FileFullName = fn;


            Thread.Sleep(100);

            Assert.AreEqual(fn, Logger.FileFullName, true);
        }

        [TestMethod]
        public void TestConvertLogLevelLogger()
        {


            Assert.AreEqual(Logger.ConvertToLogLevel("Debug"), LogLevel.Debug);
            Assert.AreEqual(Logger.ConvertToLogLevel("error"), LogLevel.Error);
            Assert.AreEqual(Logger.ConvertToLogLevel("fatal"), LogLevel.Fatal);
            Assert.AreEqual(Logger.ConvertToLogLevel("info"), LogLevel.Info);
            Assert.AreEqual(Logger.ConvertToLogLevel("warning"), LogLevel.Warn);
            Assert.AreEqual(Logger.ConvertToLogLevel("ASFJsadhjf"), LogLevel.Trace);
            Assert.AreEqual(Logger.ConvertToLogLevel("Trace"), LogLevel.Trace);
        }


        [TestMethod]
        public void TestSetCountFileLog()
        {
            Logger.FileCount = 5;
            Assert.AreEqual(Logger.FileCount, 5);
            Logger.FileCount = 5;
            Assert.AreEqual(Logger.FileCount, 5);

        }

        [TestMethod]
        public void TestWaitLastMessageFileLog()
        {
            TestSetFileNameLogger();
            Logger.LogLevel = LogLevel.Trace;
            var log = Logger.GetLogger();
            try
            {
                throw new Exception("error");
            }
            catch (Exception ex)
            {
                Logger.Write(ex);

                Logger.WriteDebug(ex);
                Logger.WriteTrace(ex);
                Logger.WriteWarning(ex);
                Logger.WriteInfo(ex);
                Logger.WriteError(ex);
                Logger.WriteFatal(ex);

                Logger.WriteDebug((object)ex);
                Logger.WriteTrace((object)ex);
                Logger.WriteWarning((object)ex);
                Logger.WriteInfo((object)ex);
                Logger.WriteError((object)ex);
                Logger.WriteFatal((object)ex);



                log.Write(ex);

                log.WriteDebug(ex);
                log.WriteTrace(ex);
                log.WriteWarning(ex);
                log.WriteInfo(ex);
                log.WriteError(ex);
                log.WriteFatal(ex);

                log.WriteDebug((object)ex);
                log.WriteTrace((object)ex);
                log.WriteWarning((object)ex);
                log.WriteInfo((object)ex);
                log.WriteError((object)ex);
                log.WriteFatal((object)ex);


                log.Write(ex, "Test");

            }
            var text = Logger.GetLastMessage(LogLevel.Default);
            Assert.AreEqual(text.Contains("error"), true);
            text = Logger.GetLastMessage(LogLevel.Fatal);
            Assert.AreEqual(text.Contains("error"), true);
            text = Logger.GetLastMessage(LogLevel.Info);
            Assert.AreEqual(text.Contains("error"), true);
            text = Logger.GetLastMessage(LogLevel.Debug);
            Assert.AreEqual(text.Contains("error"), true);
            text = Logger.GetLastMessage(LogLevel.Trace);
            Assert.AreEqual(text.Contains("error"), true);
            text = Logger.GetLastMessage(LogLevel.Warn);
            Assert.AreEqual(text.Contains("error"), true);
            text = Logger.GetLastMessage(LogLevel.Error);
            Assert.AreEqual(text.Contains("error"), true);
            text = Logger.GetLastMessage(LogLevel.Fatal);
            Assert.AreEqual(string.IsNullOrEmpty(text), true);

        }


        [TestMethod]
        public void TestSetSizeFileLog()
        {
            Logger.FileSize = 1;
            Assert.AreEqual(Logger.FileSize, 1);
            Logger.FileSize = 1;
            Assert.AreEqual(Logger.FileSize, 1);

        }


        [TestMethod]
        public void TestSetDeepFileLog()
        {
            Logger.DeepParse = 7;
            Assert.AreEqual(Logger.DeepParse, 7);
            Logger.DeepParse = 5;
            Assert.AreEqual(Logger.DeepParse, 5);
        }


        [TestMethod]
        public void TestSetCommpresFileLog()
        {
            Logger.Compression = false;
            Assert.AreEqual(Logger.Compression, false);
        }

        [TestMethod]
        public void TestSetLevelFileLog()
        {
            Logger.LogLevel = LogLevel.Debug;
            Assert.AreEqual(Logger.LogLevel, LogLevel.Debug);

            Logger.LogLevel = LogLevel.Error;
            Assert.AreEqual(Logger.LogLevel, LogLevel.Error);

            Logger.LogLevel = LogLevel.Fatal;
            Assert.AreEqual(Logger.LogLevel, LogLevel.Fatal);

            Logger.LogLevel = LogLevel.Info;
            Assert.AreEqual(Logger.LogLevel, LogLevel.Info);

            Logger.LogLevel = LogLevel.Off;
            Assert.AreEqual(Logger.LogLevel, LogLevel.Off);

            Logger.LogLevel = LogLevel.Trace;
            Assert.AreEqual(Logger.LogLevel, LogLevel.Trace);

            Logger.LogLevel = LogLevel.Warn;
            Assert.AreEqual(Logger.LogLevel, LogLevel.Warn);

            Logger.LogLevel = LogLevel.Default;
            Assert.AreEqual(Logger.LogLevel, LogLevel.Default);


        }


        [TestMethod]
        public void TestSetAliasFileLog()
        {
            TestSetFileNameLogger();

            Directory.CreateDirectory(_dir);

            Logger.SetAliasFile("alias.log", "test");

            Logger.SetAliasFile("alias.log", "Test1", 10, 1, LogLevel.Info);

            Logger.SetAlias("Test1");
            Logger.Write(LogLevel.Trace, "test1");

            Thread.Sleep(100);
            Logger.Flush();
            for (int j = 0; j < 6; j++)
            {
                Logger.Error(bigtext);
                for (int i = 0; i < 100; i++)
                {
                    if (File.Exists($@"{_dir}\alias.log"))
                    {
                        break;
                    }
                    Thread.Sleep(20);
                }
            }
            var files = Directory.GetFiles($@"{_dir}", "alias*.log");

            Assert.AreEqual(files.Length, 1);

        }


        [TestMethod]
        public void TestGenerateObjectFileLog()
        {
            TestSetFileNameLogger();
            var logger = Logger.GetLogger();
            var logger1 = Logger.GetLogger("test");

            CompareLogic compareLogic = new CompareLogic();
            ComparisonResult result;

            result = compareLogic.Compare(logger, logger1);

            Assert.AreEqual(result.AreEqual, true);

            logger1 = Logger.GetLogger("test1", 10, 100, LogLevel.Fatal);
        }
        [TestMethod]
        public void TestWriteFileLog()
        {
            TestSetFileNameLogger();
            var log = Logger.GetLogger();
            var text = "data";
            foreach (LogLevel lev in Enum.GetValues(typeof(LogLevel)))
            {
                TestWriteFileLog(text, lev, log);
                text = $"{text} data";
            }
        }


        [TestMethod]
        public void TestCountUncompressionFileLog()
        {
            TestSetFileNameLogger();

            Logger.FileSize = 1;
            Logger.FileCount = 5;
            Logger.LogLevel = LogLevel.Trace;
            Logger.Compression = false;

            for (int i = 0; i < 5; i++)
            {

                var files1 = Directory.GetFiles($@"{_dir}", "*.*");
                foreach (var item in files1)
                {
                    if (File.Exists(item))
                    {

                        File.SetAttributes(item, FileAttributes.Normal);
                        File.Delete(item);
                    }
                }
                Thread.Sleep(10);
            }


            Thread.Sleep(1000);
            Logger.Error(bigtext);
            Logger.Flush();
            for (int j = 0; j < 6; j++)
            {
                Logger.Error(bigtext);
                for (int i = 0; i < 20; i++)
                {
                    if (File.Exists($@"{_dir}\test.0{j}.log"))
                    {
                        break;
                    }
                    Thread.Sleep(25);
                }
            }
            Logger.Flush();
            var files = Directory.GetFiles($@"{_dir}", "test*.log");

            Assert.AreEqual(6, files.Length);

        }

        [TestMethod]
        public void TestFatalFileLog()
        {
            TestSetFileNameLogger();

            Directory.CreateDirectory(_dir);

            var data = "Data";

            TestSetFileNameLogger();
            var log = Logger.GetLogger();
            var level = LogLevel.Fatal;

            foreach (LogLevel lev in Enum.GetValues(typeof(LogLevel)))
            {
                if (lev == LogLevel.Off)
                {
                    continue;
                }
                if (lev == LogLevel.Default)
                {
                    continue;
                }


                for (int i = 0; i < 5; i++)
                {
                    var files = Directory.GetFiles($@"{_dir}", "*.log");
                    foreach (var item in files)
                    {
                        try
                        {
                            if (File.Exists(item))
                            {
                                File.SetAttributes(item, FileAttributes.Normal);
                                File.Delete(item);
                            }
                        }
                        catch
                        {
                            Thread.Sleep(50);
                        }
                    }
                }

                Logger.LogLevel = lev;


                log.Fatal(data);
                log.WriteFatal(data);


                Logger.Fatal(data);
                Logger.WriteFatal(data);
                var text = "";
                Logger.Flush();
                for (int i = 0; i < 10; i++)
                {
                    try
                    {
                        if (File.Exists($@"{_dir}\test.log"))
                        {
                            text = File.ReadAllText($@"{_dir}\test.log");
                            break;
                        }
                        Thread.Sleep(30);
                    }
                    catch
                    {
                        Thread.Sleep(500);
                    }
                }
                Assert.AreEqual(text.Contains(data), (int)lev >= (int)level);
                Assert.AreEqual(text.Contains(level.ToString().ToUpper()), (int)lev >= (int)level);
            }
        }


        [TestMethod]
        public void TestErrorFileLog()
        {
            TestSetFileNameLogger();

            Directory.CreateDirectory(_dir);
            TestSetFileNameLogger();
            var log = Logger.GetLogger();

            var data = "Data";
            var level = LogLevel.Error;

            foreach (LogLevel lev in Enum.GetValues(typeof(LogLevel)))
            {
                if (lev == LogLevel.Off)
                {
                    continue;
                }
                if (lev == LogLevel.Default)
                {
                    continue;
                }


                for (int i = 0; i < 5; i++)
                {
                    var files = Directory.GetFiles($@"{_dir}", "*.log");
                    foreach (var item in files)
                    {
                        try
                        {
                            if (File.Exists(item))
                            {

                                File.SetAttributes(item, FileAttributes.Normal);
                                File.Delete(item);
                            }
                        }
                        catch
                        {
                            Thread.Sleep(500);
                        }
                    }
                }

                Logger.LogLevel = lev;

                log.Error(data);
                log.WriteError(data);

                Logger.Error(data);
                Logger.WriteError(data);

                Logger.Flush();
                var text = "";
                try
                {
                    if (File.Exists($@"{_dir}\test.log"))
                    {
                        text = File.ReadAllText($@"{_dir}\test.log");
                        break;
                    }
                }
                catch
                {
                }

                Assert.AreEqual(text.Contains(data), (int)lev >= (int)level);
                Assert.AreEqual(text.Contains(level.ToString().ToUpper()), (int)lev >= (int)level);
            }
        }


        [TestMethod]
        public void TestWarnFileLog()
        {
            TestSetFileNameLogger();

            Directory.CreateDirectory(_dir);

            TestSetFileNameLogger();
            var log = Logger.GetLogger();

            var data = "Data";

            var level = LogLevel.Warn;

            foreach (LogLevel lev in Enum.GetValues(typeof(LogLevel)))
            {
                if (lev == LogLevel.Off)
                {
                    continue;
                }
                if (lev == LogLevel.Default)
                {
                    continue;
                }


                for (int i = 0; i < 5; i++)
                {
                    var files = Directory.GetFiles($@"{_dir}", "*.log");
                    foreach (var item in files)
                    {
                        try
                        {
                            if (File.Exists(item))
                            {

                                File.SetAttributes(item, FileAttributes.Normal);
                                File.Delete(item);
                            }
                        }
                        catch
                        {
                            Thread.Sleep(50);
                        }
                    }
                    Thread.Sleep(120);
                }

                Logger.LogLevel = lev;

                Logger.Warn(data);
                Logger.WriteWarning(data);


                log.Warning(data);
                log.WriteWarning(data);

                var text = "";
                Logger.LogLevel = lev;

                Logger.Flush();

                try
                {
                    if (File.Exists($@"{_dir}\test.log"))
                    {
                        text = File.ReadAllText($@"{_dir}\test.log");
                        break;
                    }
                }
                catch
                {

                }

                Assert.AreEqual(text.Contains(data), (int)lev >= (int)level);
                Assert.AreEqual(text.Contains(level.ToString().ToUpper()), (int)lev >= (int)level);
            }
        }


        [TestMethod]
        public void TestInfoFileLog()
        {
            TestSetFileNameLogger();

            Directory.CreateDirectory(_dir);

            var data = "Data";

            var level = LogLevel.Info;
            TestSetFileNameLogger();
            var log = Logger.GetLogger();

            foreach (LogLevel lev in Enum.GetValues(typeof(LogLevel)))
            {
                if (lev == LogLevel.Off)
                {
                    continue;
                }
                if (lev == LogLevel.Default)
                {
                    continue;
                }


                for (int i = 0; i < 5; i++)
                {
                    var files = Directory.GetFiles($@"{_dir}", "*.log");
                    foreach (var item in files)
                    {
                        try
                        {
                            if (File.Exists(item))
                            {

                                File.SetAttributes(item, FileAttributes.Normal);
                                File.Delete(item);
                            }
                        }
                        catch
                        {
                            Thread.Sleep(50);
                        }
                    }
                    Thread.Sleep(120);
                }


                Logger.LogLevel = lev;
                Logger.Info(data);
                Logger.WriteInfo(data);


                log.Info(data);
                log.WriteInfo(data);
                var text = "";
                Logger.Flush();
                try
                {
                    if (File.Exists($@"{_dir}\test.log"))
                    {
                        text = File.ReadAllText($@"{_dir}\test.log");
                        break;
                    }
                }
                catch
                {
                    Thread.Sleep(500);
                }
                Assert.AreEqual(text.Contains(data), (int)lev >= (int)level);
                Assert.AreEqual(text.Contains(level.ToString().ToUpper()), (int)lev >= (int)level);
            }
        }


        static object[] Values = new object[] { null
            , new object[] { new string[] {"1","2","3" }, 1, 10, null }
            , new StringBuilder("sdfsdfgsdgsd")
            , "hjfhsadfs\n\roiasfaiosfvchasd/n/rkaofjskdf"
            , new Dictionary<string, string>() { { "1", "2" }, { "2", "3" } }
            , Logger.GetLogger()
        };





        [TestMethod]
        public void TestFileObjectLog()
        {
            TestSetFileNameLogger();

            Directory.CreateDirectory(_dir);
            TestSetFileNameLogger();
            var log = Logger.GetLogger();


            foreach (LogLevel curLev in Enum.GetValues(typeof(LogLevel)))
            {
                if (curLev == LogLevel.Off)
                {
                    continue;
                }
                if (curLev == LogLevel.Default)
                {
                    continue;
                }
                TestSetFileNameLogger();
                TestSetCountFileLog();
                TestSetDeepFileLog();
                TestSetCommpresFileLog();
                Logger.FileSize = 10 * 1024 * 1024;
                Logger.LogNonPublic = true;
                var level = curLev;
                foreach (var obj in Values)
                {
                    var data = obj;

                    //foreach (LogLevel lev in Enum.GetValues(typeof(LogLevel)))
                    {
                        LogLevel lev = LogLevel.Trace;
                        /*if (lev == LogLevel.Off)
                        {
                            continue;
                        }*/
                        if (lev == LogLevel.Default)
                        {
                            continue;
                        }

                        Logger.LogLevel = lev;





                        switch (curLev)
                        {
                            case LogLevel.Trace:
                                Logger.Trace(data);
                                Logger.WriteTrace(data);

                                log.Trace(data);
                                log.WriteTrace(data);

                                break;
                            case LogLevel.Debug:
                                Logger.Debug(data);
                                Logger.WriteDebug(data);

                                log.Debug(data);
                                log.WriteDebug(data);

                                break;
                            case LogLevel.Info:
                                Logger.Info(data);
                                Logger.WriteInfo(data);

                                log.Info(data);
                                log.WriteInfo(data);

                                break;
                            case LogLevel.Warn:
                                Logger.Warn(data);
                                Logger.WriteWarning(data);

                                log.Warning(data);
                                log.WriteWarning(data);

                                break;
                            case LogLevel.Error:
                                Logger.Error(data);
                                Logger.WriteError(data);

                                log.Error(data);
                                log.WriteError(data);

                                break;
                            case LogLevel.Fatal:
                                Logger.WriteFatal(data);
                                Logger.Fatal(data);

                                log.WriteFatal(data);
                                log.Fatal(data);

                                break;
                            case LogLevel.Off:
                            case LogLevel.Default:
                                break;
                            default:
                                break;
                        }



                        Logger.Write(curLev, data);
                        log.Write(curLev, data);
                        if (data is Array)
                            log.Write(curLev, (object[])data);
                    }
                }
            }
        }



        [TestMethod]
        public void TestDebugFileLog()
        {
            TestSetFileNameLogger();

            Directory.CreateDirectory(_dir);
            TestSetFileNameLogger();

            var data = "Data";
            var level = LogLevel.Debug;

            var log = Logger.GetLogger();

            foreach (LogLevel lev in Enum.GetValues(typeof(LogLevel)))
            {
                if (lev == LogLevel.Off)
                {
                    continue;
                }
                if (lev == LogLevel.Default)
                {
                    continue;
                }


                for (int i = 0; i < 5; i++)
                {
                    var files = Directory.GetFiles($@"{_dir}", "*.log");
                    foreach (var item in files)
                    {
                        try
                        {
                            if (File.Exists(item))
                            {

                                File.SetAttributes(item, FileAttributes.Normal);
                                File.Delete(item);
                            }
                        }
                        catch
                        {
                            Thread.Sleep(50);
                        }
                    }
                    Thread.Sleep(200);
                }

                Logger.LogLevel = lev;
                log.Debug(data);
                log.WriteDebug(data);
                Logger.Debug(data);
                Logger.WriteDebug(data);
                var text = "";
                Logger.Flush();
                try
                {
                    if (File.Exists($@"{_dir}\test.log"))
                    {
                        text = File.ReadAllText($@"{_dir}\test.log");
                        break;
                    }
                }
                catch
                {

                }

                Assert.AreEqual(text.Contains(data), (int)lev >= (int)level);
                Assert.AreEqual(text.Contains(level.ToString().ToUpper()), (int)lev >= (int)level);
            }
        }

        [TestMethod]
        public void TestTraceFileLog()
        {
            TestSetFileNameLogger();

            Directory.CreateDirectory(_dir);

            var data = "Data";

            var level = LogLevel.Trace;
            TestSetFileNameLogger();

            var log = Logger.GetLogger();


            foreach (LogLevel lev in Enum.GetValues(typeof(LogLevel)))
            {
                if (lev == LogLevel.Off)
                {
                    continue;
                }
                if (lev == LogLevel.Default)
                {
                    continue;
                }
                for (int i = 0; i < 5; i++)
                {
                    var files = Directory.GetFiles($@"{_dir}", "*.log");
                    foreach (var item in files)
                    {
                        try
                        {
                            if (File.Exists(item))
                            {

                                File.SetAttributes(item, FileAttributes.Normal);
                                File.Delete(item);
                            }
                        }
                        catch
                        {
                            Thread.Sleep(50);
                        }
                    }
                    Thread.Sleep(200);
                }



                Logger.LogLevel = lev;
                Logger.Trace(data);
                Logger.WriteTrace(data);

                log.Trace(data);
                log.WriteTrace(data);
                Logger.Flush();

                var text = "";
                try
                {
                    if (File.Exists($@"{_dir}\test.log"))
                    {
                        text = File.ReadAllText($@"{_dir}\test.log");
                        break;
                    }
                }
                catch
                {
                }
                Assert.AreEqual(text.Contains(data), (int)lev >= (int)level);
                Assert.AreEqual(text.Contains(level.ToString().ToUpper()), (int)lev >= (int)level);
            }
        }

        [TestMethod]
        public void TestWriteFileDataLog()
        {

            TestSetFileNameLogger();
            var log = Logger.GetLogger();


            foreach (LogLevel lev in Enum.GetValues(typeof(LogLevel)))
            {
                using (var ds = new DataSet("DataSet"))
                {

                    DataTable table = new DataTable();

                    // Declare DataColumn and DataRow variables.
                    DataColumn column;
                    DataRow row;


                    // Create new DataColumn, set DataType, ColumnName and add to DataTable.    
                    column = new DataColumn
                    {
                        DataType = System.Type.GetType("System.Int32"),
                        ColumnName = "id"
                    };
                    table.Columns.Add(column);

                    // Create second column.
                    column = new DataColumn
                    {
                        DataType = Type.GetType("System.String"),
                        ColumnName = "item"
                    };
                    table.Columns.Add(column);

                    // Create new DataRow objects and add to DataTable.    
                    for (int i = 0; i < 10; i++)
                    {
                        row = table.NewRow();
                        row["id"] = i;
                        row["item"] = "item " + i.ToString();
                        table.Rows.Add(row);
                    }
                    ds.Tables.Add(table);
                    TestWriteFileLogData(ds, lev, log);
                    TestWriteFileLogData(ds.Tables[0], lev, log);
                    TestWriteFileLogData(ds.Tables[0].Rows[0], lev, log);
                }

            }
        }


        public void TestWriteFileLogData(object data, LogLevel level, DataLogger log)
        {


            if (level == LogLevel.Off)
            {
                return;
            }
            if (level == LogLevel.Default)
            {
                return;
            }



            Directory.CreateDirectory(_dir);
            {
                LogLevel lev = LogLevel.Trace;



                for (int i = 0; i < 5; i++)
                {
                    var files = Directory.GetFiles($@"{_dir}", "*.log");
                    foreach (var item in files)
                    {
                        try
                        {
                            if (File.Exists(item))
                            {

                                File.SetAttributes(item, FileAttributes.Normal);
                                File.Delete(item);
                            }
                        }
                        catch
                        {
                            Thread.Sleep(50);
                        }
                    }
                    Thread.Sleep(120);
                }


                Logger.LogLevel = lev;
                Logger.Write(level, data);


                log.Write(level, data);

                Logger.Flush();
                for (int i = 0; i < 10; i++)
                {
                    if (File.Exists($@"{_dir}\test.log"))
                    {

                        break;
                    }
                    Thread.Sleep(50);
                }

                var files1 = Directory.GetFiles($@"{_dir}", "test*.log");

                Assert.AreEqual(files1.Length >= 1, (int)lev >= (int)level);

            }

        }

        public void TestWriteFileLog(string data, LogLevel level, DataLogger log)
        {
            TestSetFileNameLogger();

            if (level == LogLevel.Off)
            {
                return;
            }
            if (level == LogLevel.Default)
            {
                return;
            }

            var dir = @"c:\sni\namos\trace";
            Directory.CreateDirectory(dir);

            foreach (LogLevel lev in Enum.GetValues(typeof(LogLevel)))
            {

                if (lev == LogLevel.Default)
                {
                    continue;
                }





                Logger.LogLevel = lev;
                Logger.Write(level, data);
                Logger.Write(level, data.Split(' '));


                log.Write(level, data);
                log.Write(level, data.Split(' '));

                var text = "";
                for (int i = 0; i < 10; i++)
                {
                    try
                    {
                        if (File.Exists($@"{dir}\test.log"))
                        {
                            text = File.ReadAllText($@"{dir}\test.log");
                            break;
                        }
                        Thread.Sleep(20);
                    }
                    catch
                    {
                        Thread.Sleep(500);
                    }
                }



            }

        }


        const string bigtext = @"<ROOT xmlns = """" >

    < RunOn Type=""HOS"">
		<Service Name = ""DataConnection"" Path=""c:\SNI\NamosRus\ManageServices\ManageDataConnection.exe"" Errors=""0"" LogLevel=""Debug"" LogCount=""25"">
			<!--<MasterDepended>MSSQLSERVER</MasterDepended>-->
		</Service>
		<Service Name = ""FileReceiver"" Path=""c:\SNI\NamosRus\ManageServices\ManageFileReceiver.exe"" Errors=""0"" LogLevel=""Debug""></Service>
		<Service Name = ""FileSender"" Path=""c:\SNI\NamosRus\ManageServices\ManageFileSender.exe"" Errors=""0"" LogLevel=""Debug"" ></Service>
		<Service Name = ""ImportSheduler"" Path=""c:\SNI\NamosRus\ManageServices\ManageImportSheduler.exe"" Errors=""0"" LogLevel=""Debug"" >
			<Depended>DataConnection</Depended>
			<Functions>
				<Connection Service = ""DataConnection"" ></ Connection >

            </ Functions >

        </ Service >

        < Service Name=""ReportService"" Path=""c:\SNI\NamosRus\ManageServices\ManageReportService.exe""  LogLevel=""Debug""  Errors=""0"">
			<Depended>DataConnection</Depended>
			<Functions>
				<Connection Service = ""DataConnection"" ></ Connection >

            </ Functions >

        </ Service >

        < Service Name=""SecureDataConnection"" Path=""c:\SNI\NamosRus\ManageServices\ManageSecureDataConnection.exe"" Errors=""0"" LogLevel=""Debug"" >
			<Depended>DataConnection</Depended>
			<Functions>
				<Connection Service = ""DataConnection"" ></ Connection >

            </ Functions >

        </ Service >

        < Service Name=""ExportSheduler"" Path=""c:\SNI\NamosRus\ManageServices\ManageExportSheduler.exe"" Errors=""0"" LogLevel=""Debug"" >
			<Depended>DataConnection</Depended>
			<Functions>
				<Connection Service = ""DataConnection"" ></ Connection >

            </ Functions >

        </ Service >

        < Service Name=""ServerCheckSumFiles"" Path=""c:\SNI\NamosRus\ManageServices\ManageServerCheckSumFiles.exe"" Errors=""0"" LogLevel=""Debug"" ></Service>
		<Service Name = ""FileReceiveExecuting"" Path=""c:\SNI\NamosRus\ManageServices\ManageFileReceiveExecuting.exe"" Errors=""0"" LogLevel=""Debug"" ></Service>
		<Service Name = ""FileSendExecuter"" Path=""c:\SNI\NamosRus\ManageServices\ManageFileSendExecuter.exe"" Errors=""0"" LogLevel=""Debug"" ></Service>
		<Service Name = ""ServerAuditDevice"" Path=""c:\SNI\NamosRus\ManageServices\ManageServerAuditDevice.exe"" Errors=""0"" LogLevel=""Debug""></Service>
		<Service Name = ""ImportExternalData"" Path=""c:\SNI\NamosRus\ManageServices\ManageImportExternalData.exe"" Errors=""0"" LogLevel=""Debug"">
			<Depended>DataConnection</Depended>
			<Functions>
				<Connection Service = ""DataConnection"" ></ Connection >

            </ Functions >

        </ Service >

        < Service Name=""EventRegistrationAudit"" Path=""c:\SNI\NamosRus\ManageServices\ManageEventRegistrationAudit.exe"" Errors=""0"" LogLevel=""Debug"">
			<Depended>DataConnection</Depended>
			<Functions>
				<Connection Service = ""DataConnection"" ></ Connection >

            </ Functions >

        </ Service >

        < Service Name=""TaskWorker"" Path=""c:\SNI\NamosRus\ManageServices\ManageTaskWorker.exe"" Errors=""0"" LogLevel=""Debug"">
			<Depended>DataConnection</Depended>
			<Functions>
				<Connection Service = ""DataConnection"" ></ Connection >

            </ Functions >

        </ Service >

        < Service Name=""ServiceTracker"" Path=""c:\SNI\NamosRus\ManageServices\ManageServiceTracker.exe"" Errors=""0"" LogLevel=""Debug""/>
		
	</RunOn>
	<RunOn Type = ""BOS,BOSOPT,BOSOPTNIGHT,BOSPOS,BOSPOSOPT"" >

        < Service Name=""DataConnection"" Path=""c:\SNI\NamosRus\ManageServices\ManageDataConnection.exe"" Errors=""0"" LogLevel=""Debug"" LogCount=""25"" >
		</Service>
		<Service Name = ""FileReceiver"" Path=""c:\SNI\NamosRus\ManageServices\ManageFileReceiver.exe"" LogLevel=""Debug"" Errors=""0""></Service>
		<Service Name = ""FileSender"" Path=""c:\SNI\NamosRus\ManageServices\ManageFileSender.exe""  LogLevel=""Debug""  Errors=""0""></Service>
		<Service Name = ""ImportSheduler"" Path=""c:\SNI\NamosRus\ManageServices\ManageImportSheduler.exe""  LogLevel=""Debug""  Errors=""0"">
			<Depended>DataConnection</Depended>
			<Functions>
				<Connection Service = ""DataConnection"" ></ Connection >

            </ Functions >

        </ Service >

        < Service Name=""ReportService"" Path=""c:\SNI\NamosRus\ManageServices\ManageReportService.exe""  LogLevel=""Debug""  Errors=""0"">
			<Depended>DataConnection</Depended>
			<Functions>
				<Connection Service = ""DataConnection"" ></ Connection >

            </ Functions >

        </ Service >

        < Service Name=""ExportSheduler"" Path=""c:\SNI\NamosRus\ManageServices\ManageExportSheduler.exe""  LogLevel=""Debug""  Errors=""0"">
			<Depended>DataConnection</Depended>
			<Functions>
				<Connection Service = ""DataConnection"" ></ Connection >

            </ Functions >

        </ Service >

        < Service Name=""EventRegistrationAudit"" Path=""c:\SNI\NamosRus\ManageServices\ManageEventRegistrationAudit.exe""  LogLevel=""Debug""  Errors=""0"">
			<Depended>DataConnection</Depended>
			<Functions>
				<Connection Service = ""DataConnection"" ></ Connection >

            </ Functions >

        </ Service >

        < Service Name=""Replication"" Path=""c:\SNI\NamosRus\ManageServices\ManageReplication.exe"" LogLevel=""Debug""   Errors=""0"">
			<Depended>DataConnection</Depended>
			<Functions>
				<Connection Service = ""DataConnection"" ></ Connection >

            </ Functions >

        </ Service >

        < Service Name=""PinUserManager"" Path=""c:\SNI\NamosRus\ManageServices\ManagePinUserManager.exe""  LogLevel=""Debug""  Errors=""0"">
			<Depended>DataConnection</Depended>
			<Functions>
				<Connection Service = ""DataConnection"" ></ Connection >

            </ Functions >

        </ Service >

        < Service Name=""SecureDataConnection"" Path=""c:\SNI\NamosRus\ManageServices\ManageSecureDataConnection.exe"" Errors=""0"" LogLevel=""Debug"" >
			<Depended>DataConnection</Depended>
			<Functions>
				<Connection Service = ""DataConnection"" ></ Connection >

            </ Functions >

        </ Service >


        < !--

        < Service Name=""CheckSumFiles"" Path=""c:\SNI\NamosRus\ManageServices\ManageCheckSumFiles.exe""  LogLevel=""Debug""  Errors=""0"">
			<Depended>DataConnection</Depended>
			<Functions>
				<Connection Service = ""DataConnection"" ></ Connection >

            </ Functions >

        </ Service >
        -->

        < Service Name=""ReconciliationExport"" Path=""c:\SNI\NamosRus\ManageServices\ManageReconciliationExport.exe""  LogLevel=""Debug""  Errors=""0"">
			<Depended>DataConnection</Depended>
			<Functions>
				<Connection Service = ""DataConnection"" ></ Connection >

            </ Functions >

        </ Service >

        < Service Name=""FileReceiveExecuting"" Path=""c:\SNI\NamosRus\ManageServices\ManageFileReceiveExecuting.exe""  LogLevel=""Debug""  Errors=""0""></Service>
		<Service Name = ""EPSSystem"" Path=""c:\SNI\NamosRus\ManageServices\EPSSystem.dll""  LogLevel=""Debug""  Errors=""0"">
			<Depended>DataConnection</Depended>
			<Functions>
				<Connection Service = ""DataConnection"" ></ Connection >

                < FullSynhronization Service=""Replication""></FullSynhronization>
			</Functions>
		</Service>
		<Service Name = ""ServiceTracker"" Path=""c:\SNI\NamosRus\ManageServices\ManageServiceTracker.exe"" Errors=""0"" LogLevel=""Debug""/>
	</RunOn>
	<RunOn Type = ""BOSOPTSTANDALONE"" >

        < Service Name=""DataConnection"" Path=""c:\SNI\NamosRus\ManageServices\ManageDataConnection.exe"" Errors=""0"" LogLevel=""Debug"" LogCount=""25"" >
		</Service>
		<Service Name = ""FileReceiver"" Path=""c:\SNI\NamosRus\ManageServices\ManageFileReceiver.exe"" LogLevel=""Debug"" Errors=""0""></Service>
		<Service Name = ""FileSender"" Path=""c:\SNI\NamosRus\ManageServices\ManageFileSender.exe""  LogLevel=""Debug""  Errors=""0""></Service>
		<Service Name = ""ImportSheduler"" Path=""c:\SNI\NamosRus\ManageServices\ManageImportSheduler.exe""  LogLevel=""Debug""  Errors=""0"">
			<Depended>DataConnection</Depended>
			<Functions>
				<Connection Service = ""DataConnection"" ></ Connection >

            </ Functions >

        </ Service >

        < Service Name=""ReportService"" Path=""c:\SNI\NamosRus\ManageServices\ManageReportService.exe""  LogLevel=""Debug""  Errors=""0"">
			<Depended>DataConnection</Depended>
			<Functions>
				<Connection Service = ""DataConnection"" ></ Connection >

            </ Functions >

        </ Service >

        < Service Name=""ExportSheduler"" Path=""c:\SNI\NamosRus\ManageServices\ManageExportSheduler.exe""  LogLevel=""Debug""  Errors=""0"">
			<Depended>DataConnection</Depended>
			<Functions>
				<Connection Service = ""DataConnection"" ></ Connection >

            </ Functions >

        </ Service >

        < Service Name=""EventRegistrationAudit"" Path=""c:\SNI\NamosRus\ManageServices\ManageEventRegistrationAudit.exe""  LogLevel=""Debug""  Errors=""0"">
			<Depended>DataConnection</Depended>
			<Functions>
				<Connection Service = ""DataConnection"" ></ Connection >

            </ Functions >

        </ Service >

        < Service Name=""Replication"" Path=""c:\SNI\NamosRus\ManageServices\ManageReplication.exe"" LogLevel=""Debug""   Errors=""0"">
			<Depended>DataConnection</Depended>
			<Functions>
				<Connection Service = ""DataConnection"" ></ Connection >

            </ Functions >

        </ Service >

        < Service Name=""PinUserManager"" Path=""c:\SNI\NamosRus\ManageServices\ManagePinUserManager.exe""  LogLevel=""Debug""  Errors=""0"">
			<Depended>DataConnection</Depended>
			<Functions>
				<Connection Service = ""DataConnection"" ></ Connection >

            </ Functions >

        </ Service >

        < Service Name=""SecureDataConnection"" Path=""c:\SNI\NamosRus\ManageServices\ManageSecureDataConnection.exe"" Errors=""0"" LogLevel=""Debug"" >
			<Depended>DataConnection</Depended>
			<Functions>
				<Connection Service = ""DataConnection"" ></ Connection >

            </ Functions >

        </ Service >


        < !--

        < Service Name=""CheckSumFiles"" Path=""c:\SNI\NamosRus\ManageServices\ManageCheckSumFiles.exe""  LogLevel=""Debug""  Errors=""0"">
			<Depended>DataConnection</Depended>
			<Functions>
				<Connection Service = ""DataConnection"" ></ Connection >

            </ Functions >

        </ Service >
        -->

        < Service Name=""ReconciliationExport"" Path=""c:\SNI\NamosRus\ManageServices\ManageReconciliationExport.exe""  LogLevel=""Debug""  Errors=""0"">
			<Depended>DataConnection</Depended>
			<Functions>
				<Connection Service = ""DataConnection"" ></ Connection >

            </ Functions >

        </ Service >

        < Service Name=""FileReceiveExecuting"" Path=""c:\SNI\NamosRus\ManageServices\ManageFileReceiveExecuting.exe""  LogLevel=""Debug""  Errors=""0""></Service>
		<Service Name = ""EPSSystem"" Path=""c:\SNI\NamosRus\ManageServices\EPSSystem.dll""  LogLevel=""Debug""  Errors=""0"">
			<Depended>DataConnection</Depended>
			<Functions>
				<Connection Service = ""DataConnection"" ></ Connection >

                < FullSynhronization Service=""Replication""></FullSynhronization>
			</Functions>
		</Service>
		<Service Name = ""TaskWorkPlanning"" Path=""c:\SNI\NamosRus\ManageServices\ManageTaskWorker.exe"" Errors=""0"" LogLevel=""Debug"">
			<Depended>DataConnection</Depended>
			<Functions>
				<Connection Service = ""DataConnection"" ></ Connection >

            </ Functions >

        </ Service >

        < Service Name=""ServiceTracker"" Path=""c:\SNI\NamosRus\ManageServices\ManageServiceTracker.exe"" Errors=""0"" LogLevel=""Debug""/>		
	</RunOn>	
	<RunOn Type = ""POS,POSOPT"" >

        < Service Name=""DataConnection"" Path=""c:\SNI\NamosRus\ManageServices\ManageDataConnection.exe"" Errors=""0"" LogCount=""25"" LogLevel=""Debug"" LastClose=""True""/>
		<Service Name = ""EPSSystem"" Path=""c:\SNI\NamosRus\ManageServices\EPSSystem.dll""  LogLevel=""Debug""  Errors=""0"">
			<Depended>DataConnection</Depended>
			<Functions>
				<Connection Service = ""DataConnection"" ></ Connection >

                < FullSynhronization Service=""Replication""></FullSynhronization>
			</Functions>
		</Service>
		<Service Name = ""MFManager"" Path=""c:\SNI\NamosRus\ManageServices\MFManager.dll""  LogLevel=""Debug""  Errors=""0"">
			<Depended>DataConnection</Depended>
			<Depended>EPSSystem</Depended>
			<Functions>
				<Connection Service = ""DataConnection"" ></ Connection >

                < FullSynhronization Service=""Replication""></FullSynhronization>
			</Functions>
		</Service>		
		<Service Name = ""FileReceiver"" Path=""c:\SNI\NamosRus\ManageServices\ManageFileReceiver.exe"" LogLevel=""Debug"" Errors=""0""></Service>
		<Service Name = ""FileSender"" Path=""c:\SNI\NamosRus\ManageServices\ManageFileSender.exe""  LogLevel=""Debug""  Errors=""0""></Service>
		<Service Name = ""ItvReceiver"" Path=""c:\SNI\NamosRus\ManageServices\WN.Receiver.ManagedService.exe""  LogLevel=""Debug""  Errors=""0""></Service>
		<Service Name = ""Replication"" Path=""c:\SNI\NamosRus\ManageServices\ManageReplication.exe""  LogLevel=""Debug""  Errors=""0"">
			<Depended>DataConnection</Depended>
			<Functions>
				<Connection Service = ""DataConnection"" ></ Connection >

            </ Functions >

        </ Service >

        < Service Name=""NamosNTWatcher"" Path=""c:\SNI\NamosRus\ManageServices\ManageNamosNTWatcher.exe""  LogLevel=""Debug""  Errors=""0""></Service>
		<!--
		<Service Name = ""CheckSumFiles"" Path=""c:\SNI\NamosRus\ManageServices\ManageCheckSumFiles.exe""  LogLevel=""Debug""  Errors=""0"">
			<Depended>DataConnection</Depended>
			<Functions>
				<Connection Service = ""DataConnection"" ></ Connection >

            </ Functions >

        </ Service >
        -->

        < Service Name=""EventRegistrationAudit"" Path=""c:\SNI\NamosRus\ManageServices\ManageEventRegistrationAudit.exe""  LogLevel=""Debug""  Errors=""0"">
			<Depended>DataConnection</Depended>
			<Functions>
				<Connection Service = ""DataConnection"" ></ Connection >

            </ Functions >

        </ Service >


        < Service Name=""FileReceiveExecuting"" Path=""c:\SNI\NamosRus\ManageServices\ManageFileReceiveExecuting.exe"" LogLevel=""Debug""   Errors=""0""></Service>
		
		<Service Name = ""ZReportImport"" Path=""c:\SNI\NamosRus\ManageServices\ManageZReportImport.exe""  LogLevel=""Debug""  Errors=""0"">
			<Depended>DataConnection</Depended>
			<Functions>
				<Connection Service = ""DataConnection"" ></ Connection >

            </ Functions >

        </ Service >


        < Service Name=""MFTicketLoader"" Path=""c:\SNI\NamosRus\ManageServices\MFTicketLoader.dll""  LogLevel=""Debug""  Errors=""0"">
			<Depended>DataConnection</Depended>
			<Functions>
				<Connection Service = ""DataConnection"" ></ Connection >

            </ Functions >

        </ Service >

        < Service Name=""ServiceTracker"" Path=""c:\SNI\NamosRus\ManageServices\ManageServiceTracker.exe"" Errors=""0"" LogLevel=""Debug""/>
		
	</RunOn>
	<RunOn Type = ""OPT"" >

        < Service Name=""FileReceiver"" Path=""c:\SNI\NamosRus\ManageServices\ManageFileReceiver.exe"" LogLevel=""Debug"" Errors=""0""></Service>
		<Service Name = ""FileSender"" Path=""c:\SNI\NamosRus\ManageServices\ManageFileSender.exe""  LogLevel=""Debug""  Errors=""0""></Service>
		<Service Name = ""FileReceiveExecuting"" Path=""c:\SNI\NamosRus\ManageServices\ManageFileReceiveExecuting.exe"" LogLevel=""Debug""   Errors=""0""></Service>
		<Service Name = ""ServiceTracker"" Path=""c:\SNI\NamosRus\ManageServices\ManageServiceTracker.exe"" Errors=""0"" LogLevel=""Debug""/>
		<Service Name = ""ConfigOPT"" Path=""c:\SNI\NamosRus\ManageServices\ManageConfigOPT.exe"" Errors=""0"" LogLevel=""Debug""/>
	</RunOn>	
</ROOT>";


    }
}
