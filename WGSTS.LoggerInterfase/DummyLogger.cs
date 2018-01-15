using System;
using System.Collections.Generic;
using System.Text;


namespace WGSTS.Logger
{
    class DummyLogger : ILogger
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

        public void Write(Exception ex)
        {
         
        }

        public void Write(Exception ex, string mess)
        {
         
        }

        public void Write(LogLevel level, object mess)
        {
         
        }

        public void Write(LogLevel level, object[] messArray)
        {
         
        }

        public void Write(LogLevel level, string message)
        {

        }

        public void Write(LogLevel level, string[] messArray)
        {

        }

        public void WriteDebug(Exception ex)
        {

        }

        public void WriteDebug(object messArray)
        {

        }

        public void WriteDebug(string data)
        {

        }

        public void WriteError(Exception ex)
        {

        }

        public void WriteError(object messArray)
        {

        }

        public void WriteError(string data)
        {

        }

        public void WriteFatal(Exception ex)
        {

        }

        public void WriteFatal(object messArray)
        {

        }

        public void WriteFatal(string data)
        {

        }

        public void WriteInfo(Exception ex)
        {

        }

        public void WriteInfo(object messArray)
        {

        }

        public void WriteInfo(string data)
        {

        }

        public void WriteTrace(Exception ex)
        {

        }

        public void WriteTrace(object messArray)
        {

        }

        public void WriteTrace(string data)
        {

        }

        public void WriteWarning(Exception ex)
        {

        }

        public void WriteWarning(object messArray)
        {

        }

        public void WriteWarning(string data)
        {

        }
    }
}
