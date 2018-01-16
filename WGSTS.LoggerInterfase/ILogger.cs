using System;

namespace WGSTS.Logger
{
    public interface ILogger
    {
        void Debug(params object[] messArray);
        void Debug(params string[] messArray);
        void Error(params object[] messArray);
        void Error(params string[] messArray);
        void Fatal(params object[] messArray);
        void Fatal(params string[] messArray);
        void Flush();
        ILogger GetLogger(string fileName, int filecount = -1, int filesize = -1, LogLevel level = LogLevel.Default);
        void Info(params object[] messArray);
        void Info(params string[] messArray);
        void Trace(params object[] messArray);
        void Trace(params string[] messArray);
        void Warning(params object[] messArray);
        void Warning(params string[] messArray);
    }
}
