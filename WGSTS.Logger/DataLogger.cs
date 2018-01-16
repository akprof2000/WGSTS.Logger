using System;

namespace WGSTS.Logger
{
    public class DataLogger : ILogger
    {
        private string _alias;

        internal DataLogger(string alias)
        {
            _alias = alias;
        }  

        public void Fatal(params string[] messArray)
        {
            Logger.SetAlias(_alias);
            Logger.FatalA(messArray);
        }

        public void Error(params string[] messArray)
        {
            Logger.SetAlias(_alias);
            Logger.ErrorA(messArray);
        }

        public void Warning(params string[] messArray)
        {
            Logger.SetAlias(_alias);
            Logger.WarnA(messArray);
        }

        public void Info(params string[] messArray)
        {
            Logger.SetAlias(_alias);
            Logger.InfoA(messArray);
        }

        public void Trace(params string[] messArray)
        {
            Logger.SetAlias(_alias);
            Logger.TraceA(messArray);
        }

        public void Debug(params string[] messArray)
        {
            Logger.SetAlias(_alias);
            Logger.DebugA(messArray);
        }
        
        public void Fatal(params object[] messArray)
        {
            Logger.SetAlias(_alias);
            Logger.FatalA(messArray);

        }

        public void Error(params object[] messArray)
        {
            Logger.SetAlias(_alias);
            Logger.ErrorA(messArray);

        }
        public void Warning(params object[] messArray)
        {
            Logger.SetAlias(_alias);
            Logger.WarnA(messArray);

        }

        public void Info(params object[] messArray)
        {
            Logger.SetAlias(_alias);
            Logger.InfoA(messArray);

        }

        public void Debug(params object[] messArray)
        {

            Logger.SetAlias(_alias);
            Logger.DebugA(messArray);

        }

        public void Trace(params object[] messArray)
        {
            Logger.SetAlias(_alias);
            Logger.TraceA(messArray);
        }

        public void Flush()
        {
            Logger.Flush();
        }
        
        public ILogger GetLogger(string fileName, int filecount, int filesize, LogLevel level)
        {
            return Logger.GetLogger(fileName, filecount, filesize, level);
        }
    }

}
