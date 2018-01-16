using System;
using System.Collections.Generic;
using System.Text;


namespace WGSTS.Logger
{
    public class DummyLogger : ILogger
    {
        public void Debug(params object[] messArray)
        {
            
        }

        public void Debug(params string[] messArray)
        {
            
        }

        public void Error(params object[] messArray)
        {
            
        }

        public void Error(params string[] messArray)
        {
            
        }

        public void Fatal(params object[] messArray)
        {
            
        }

        public void Fatal(params string[] messArray)
        {
            
        }

        public void Flush()
        {
            
        }

        public ILogger GetLogger(string fileName, int filecount, int filesize, LogLevel level)
        {
            return this;
        }

        public void Info(params object[] messArray)
        {
            
        }

        public void Info(params string[] messArray)
        {
            
        }

        public void Trace(params object[] messArray)
        {
            
        }

        public void Trace(params string[] messArray)
        {
            
        }

        public void Warning(params object[] messArray)
        {
            
        }

        public void Warning(params string[] messArray)
        {
            
        }
    }
}
