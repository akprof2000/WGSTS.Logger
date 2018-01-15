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
        void Info(params object[] messArray);
        void Info(params string[] messArray);
        void Trace(params object[] messArray);
        void Trace(params string[] messArray);
        void Warning(params object[] messArray);
        void Warning(params string[] messArray);
        void Write(Exception ex);
        void Write(Exception ex, string mess);
        void Write(LogLevel level, object mess);
        void Write(LogLevel level, object[] messArray);
        void Write(LogLevel level, string message);
        void Write(LogLevel level, string[] messArray);
        void WriteDebug(Exception ex);
        void WriteDebug(object messArray);
        void WriteDebug(string data);
        void WriteError(Exception ex);
        void WriteError(object messArray);
        void WriteError(string data);
        void WriteFatal(Exception ex);
        void WriteFatal(object messArray);
        void WriteFatal(string data);
        void WriteInfo(Exception ex);
        void WriteInfo(object messArray);
        void WriteInfo(string data);
        void WriteTrace(Exception ex);
        void WriteTrace(object messArray);
        void WriteTrace(string data);
        void WriteWarning(Exception ex);
        void WriteWarning(object messArray);
        void WriteWarning(string data);
        void Flush();
        ILogger GetLogger(string fileName, int filecount = -1, int filesize = -1, LogLevel level = LogLevel.Default);

    }
}
