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

        public void Write(LogLevel level, string message)
        {
            Logger.Write(LogLevel.Error, message, _alias);
        }

        public void WriteError(Exception ex)
        {
            Logger.Write(LogLevel.Error, ex, _alias);
        }

        public void WriteWarning(Exception ex)
        {
            Logger.Write(LogLevel.Warn, ex, _alias);
        }

        public void WriteInfo(Exception ex)
        {
            Logger.Write(LogLevel.Info, ex, _alias);
        }


        public void WriteTrace(Exception ex)
        {
            Logger.Write(LogLevel.Trace, ex, _alias);
        }

        public void WriteDebug(Exception ex)
        {
            Logger.Write(LogLevel.Debug, ex, _alias);
        }


        public void WriteFatal(Exception ex)
        {
            Logger.Write(LogLevel.Fatal, ex, _alias);
        }
        
        public void Write(Exception ex)
        {
            Logger.Write(LogLevel.Fatal, ex, _alias);
        }

                
        public void WriteFatal(string data)
        {
            Logger.WriteFatal(data, _alias);
        }

        public void WriteError(string data)
        {
            Logger.WriteError(data, _alias);
        }

        public void WriteWarning(string data)
        {
            Logger.WriteWarning(data, _alias);
        }

        public void WriteInfo(string data)
        {
            Logger.WriteInfo(data, _alias);
        }

        public void WriteDebug(string data)
        {
            Logger.WriteDebug(data, _alias);
        }

        public void WriteTrace(string data)
        {
            Logger.WriteTrace(data, _alias);
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



        public void WriteDebug(object messArray)
        {
            Logger.WriteDebug(messArray, _alias);
        }


        public void WriteTrace(object messArray)
        {
            Logger.WriteTrace(messArray, _alias);
        }


        public void WriteError(object messArray)
        {
            Logger.WriteError(messArray, _alias);
        }


        public void WriteFatal(object messArray)
        {
            Logger.WriteFatal(messArray, _alias);
        }

        public void WriteWarning(object messArray)
        {
            Logger.WriteWarning(messArray, _alias);
        }

        public void WriteInfo(object messArray)
        {
            Logger.WriteInfo(messArray, _alias);
        }      

        public void Write(LogLevel level, string[] messArray)
        {
            Logger.Write(level, messArray, _alias);
        }

        public void Write(LogLevel level, object mess)
        {
            Logger.Write(level, mess, _alias);
        }

        public void Write(LogLevel level, object[] messArray)
        {
            Logger.Write(level, messArray, _alias);
        }


        public void Write(Exception ex, string mess)
        {
            Logger.Write(ex, mess, _alias);
        }

        public void Flush()
        {
            Logger.Flush();
        }
    }

}
