using System;
using System.Collections.Generic;
using WGSTS.Logger;

namespace TestLogger
{
    class Program
    {
        static void c()
        {

            Logger.Info("Info CCC");
            _log.Info("Info CCC");
        }
        static void b()
        {
            Logger.Trace("Trace BBB");
            _log.Trace("Trace BBB");
            Logger.Debug("Debug BBB");
            _log.Debug("Debug BBB");
            Logger.Info("Info BBB");
            _log.Info("Info BBB");
            c();
            Logger.Warn("Warn BBB");
            _log.Warning("Warn BBB");
            Logger.Error("Error BBB");
            _log.Error("Error BBB");
            Logger.Fatal("Fatal BBB");
            _log.Fatal("Fatal BBB");
        }
        static void a()
        {
            Logger.Trace("Trace AAA");
            _log.Trace("Trace AAA");
            Logger.Debug("Debug AAA");
            _log.Debug("Debug AAA");
            Logger.Info("Info AAA");
            _log.Info("Info AAA");
            b();
            Logger.Warn("Warn AAA");
            _log.Warning("Warn AAA");
            Logger.Error("Error AAA");
            _log.Error("Error AAA");
            Logger.Fatal("Fatal AAA");
            _log.Fatal("Fatal AAA");
        }

        static ILogger _log;

        public static void Main(string[] args)
        {
            Logger.LogLevel = LogLevel.Info;
            _log = Logger.GetLogger();

            Logger.Trace("This is a Trace message", "a", "b", "c");
            _log.Trace("This is a Trace message", "a", "b", "c");
            Logger.Debug("This is a Debug message");
            _log.Debug("This is a Debug message");
            Logger.Info("This is an Info message");
            _log.Info("This is an Info message");
            a();
            Logger.Warn("This is a Warn message");
            _log.Warning("This is a Warn message");
            Logger.Error("This is an Error message");
            _log.Error("This is an Error message");
            Logger.Fatal("This is a Fatal error message");
            _log.Fatal("This is a Fatal error message");

            var logger = Logger.GetLogger("SecondLog.log");
            try
            {
                first();

            }
            catch (Exception ex)
            {
                logger.Fatal(ex);
                _log.Fatal(ex);
            }
            Logger.Trace("End");
            _log.Trace("End");

            Logger.LogLevel = LogLevel.Info;
            
            Logger.Info("Write");
            _log.Info("Write");
            Logger.Trace("NotWrite");
            _log.Trace("NotWrite");


            var cl = new TestData() { Test1 = 10, Test2 = "100" };

            var list = new Dictionary<int, string>
            {
                [1] = "1",
                [2] = "2"
            };
            Logger.Info(cl);
            _log.Info(cl);
            Logger.Info(cl.ToJson());
            _log.Info(cl.ToJson());
            Logger.Info(list.ToJson());
            _log.Info(cl.ToJson());
            Console.ReadKey();
        }

        private static void first(int level = 0)
        {
            if (level < 100)
                first(++level);
            throw new Exception("test " + level.ToString());
        }

    }

    class TestData
    {
        public int Test1 { get; set; }
        public string Test2 { get; set; }
    }

}
