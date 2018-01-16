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
            Logger.Fatal(messArray);
        }

        public void Error(params string[] messArray)
        {
            Logger.SetAlias(_alias);
            Logger.Error(messArray);
        }

        public void Warning(params string[] messArray)
        {
            Logger.SetAlias(_alias);
            Logger.Warn(messArray);
        }

        public void Info(params string[] messArray)
        {
            Logger.SetAlias(_alias);
            Logger.Info(messArray);
        }

        public void Trace(params string[] messArray)
        {
            Logger.SetAlias(_alias);
            Logger.Trace(messArray);
        }

        public void Debug(params string[] messArray)
        {
            Logger.SetAlias(_alias);
            Logger.Debug(messArray);
        }
        
        public void Fatal(params object[] messArray)
        {
            Logger.SetAlias(_alias);
            Logger.Fatal(messArray);

        }

        public void Error(params object[] messArray)
        {
            Logger.SetAlias(_alias);
            Logger.Error(messArray);

        }
        public void Warning(params object[] messArray)
        {
            Logger.SetAlias(_alias);
            Logger.Warn(messArray);

        }

        public void Info(params object[] messArray)
        {
            Logger.SetAlias(_alias);
            Logger.Info(messArray);

        }

        public void Debug(params object[] messArray)
        {

            Logger.SetAlias(_alias);
            Logger.Debug(messArray);

        }

        public void Trace(params object[] messArray)
        {
            Logger.SetAlias(_alias);
            Logger.Trace(messArray);
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
