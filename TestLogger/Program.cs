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
        }
        static void b()
        {
            Logger.Trace("Trace BBB");
            Logger.Debug("Debug BBB");
            Logger.Info("Info BBB");
            c();
            Logger.Warn("Warn BBB");
            Logger.Error("Error BBB");
            Logger.Fatal("Fatal BBB");
        }
        static void a()
        {
            Logger.Trace("Trace AAA");
            Logger.Debug("Debug AAA");
            Logger.Info("Info AAA");
            b();
            Logger.Warn("Warn AAA");
            Logger.Error("Error AAA");
            Logger.Fatal("Fatal AAA");
        }
        public static void Main(string[] args)
        {
            Logger.LogLevel = LogLevel.Info;


            Logger.Trace("This is a Trace message", "a", "b", "c");
            Logger.Debug("This is a Debug message");
            Logger.Info("This is an Info message");
            a();
            Logger.Warn("This is a Warn message");
            Logger.Error("This is an Error message");
            Logger.Fatal("This is a Fatal error message");

            var logger = Logger.GetLogger("SecondLog.log");
            try
            {
                first();

            }
            catch (Exception ex)
            {
                logger.Fatal(ex);
            }
            Logger.Trace("End");

            Logger.LogLevel = LogLevel.Info;

            Logger.Info("Write");
            Logger.Trace("NotWrite");


            var cl = new TestData() { Test1 = 10, Test2 = "100" };

            var list = new Dictionary<int, string>
            {
                [1] = "1",
                [2] = "2"
            };
            Logger.Info(cl);
            Logger.Info(cl.ToJson());
            Logger.Info(list.ToJson());
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
