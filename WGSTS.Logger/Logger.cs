using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace WGSTS.Logger
{
    public static class Logger
    {
        private const string BaseTrace = @"Logging";
        private const string Const_AliasName = "AUTO -1";

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        static int _deepparse = 6;


        public static int DeepParse
        {
            get { return _deepparse; }
            set
            {
                if (_deepparse != value)
                {
                    _deepparse = value;
                    foreach (var item in _listAlias)
                    {
                        item.Value.DeepParse = _deepparse;
                    }
                }
            }
        }

        //readonly static ConcurrentDictionary<string, FileLogData> _listNameSpace = new ConcurrentDictionary<string, FileLogData>();
        readonly static ConcurrentDictionary<string, FileLogData> _listAlias = new ConcurrentDictionary<string, FileLogData>();


        public static bool Compression
        {
            get { return NLogHelper.Compression; }
            set
            {
                NLogHelper.Compression = value;
                foreach (var item in _listAlias.Values)
                {
                    item.Init();
                }

            }
        }

        static Logger()
        {
            testConsole();

            var baseregpath = AppDomain.CurrentDomain.BaseDirectory;
            

            _listAlias[Const_AliasName] = new FileLogData() { FileName = Path.Combine(baseregpath, BaseTrace, $"{Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName)}.log"), FileCount = 10, FileSize = 1024 * 1024 * 10, Level = LogLevel.Debug, DeepParse = DeepParse };
            _basepath = Path.GetDirectoryName(_listAlias[Const_AliasName].FileName);
            _listAlias[Const_AliasName].Init();
            LogLevel = LogLevel.Debug;



            Write(LogLevel.Info, $"\n\r" +
                $"******************************************************************************** \n\r" +
                $"*                      Application logger start                                * \n\r" +
                $"********************************************************************************");


        }

        private static void testConsole()
        {
            _isConsole = Environment.UserInteractive;
            var baseregpath = AppDomain.CurrentDomain.BaseDirectory;            
            Directory.CreateDirectory(Path.Combine(baseregpath, BaseTrace));
            try
            {
                if (_isConsole)
                {
                    Console.WindowWidth = 200;
                    Console.WindowHeight = 60;
                    Console.SetBufferSize(1024, 4096);
                }
            }
            catch
            {
                _isConsole = false;
            }
        }


        static public void Flush()
        {
            NLogHelper.FlushData();
        }

        static public DataLogger GetLogger()
        {
            return GetLogger(_listAlias[Const_AliasName].FileName);

        }

        static public DataLogger GetLogger(string fileName, int filecount = -1, int filesize = -1, LogLevel level = LogLevel.Default)
        {
            var alias = Guid.NewGuid().ToString();

            _listAlias[alias] = new FileLogData() { Name = alias, FileName = Path.GetFullPath(Path.Combine(_basepath, fileName)), FileCount = filecount == -1 ? _listAlias[Const_AliasName].FileCount : filecount, FileSize = filesize == -1 ? _listAlias[Const_AliasName].FileSize : filesize, Level = level, DeepParse = DeepParse };
            if (level == LogLevel.Default)
                _listAlias[alias].SetLogLevelIfDefault(LogLevel);
            _listAlias[alias].Init();

            return new DataLogger(alias);

        }
        static public void SetAliasFile(string fileName, string alias, int filecount = -1, int filesize = -1, LogLevel level = LogLevel.Default)
        {
            if (string.IsNullOrEmpty(alias))
                return;

            _listAlias[alias] = new FileLogData() { Name = alias, FileName = Path.GetFullPath(Path.Combine(_basepath, fileName)), FileCount = filecount == -1 ? _listAlias[Const_AliasName].FileCount : filecount, FileSize = filesize == -1 ? _listAlias[Const_AliasName].FileSize : filesize, Level = level, DeepParse = DeepParse };
            if (level == LogLevel.Default)
                _listAlias[alias].SetLogLevelIfDefault(LogLevel);

            _listAlias[alias].Init();
        }


        public static int FileCount { get { return _listAlias[Const_AliasName].FileCount; } set { _listAlias[Const_AliasName].FileCount = value; } }

        readonly static ConcurrentDictionary<LogLevel, string> _lastmessage = new ConcurrentDictionary<LogLevel, string>();
        private const string File_Name = @"C:\SNI\Namos\Trace\Log.log";

        public static string GetLastMessage(LogLevel level)
        {
            try
            {

                Thread.Sleep(100);

                for (int i = 0; i < 9; i++)
                {
                    if ((level == LogLevel.Default && _lastmessage.Count > 0))
                    {
                        return _lastmessage.First().Value;
                    }
                    else if (_lastmessage.ContainsKey(level))
                    {
                        _lastmessage.TryRemove(level, out string val);
                        return val;
                    }
                    Thread.Sleep(100);
                }



            }
            catch
            {
                return null;
            }
            return null;
        }


        public static string FileFullName
        {
            get
            {
                return _listAlias[Const_AliasName].FileName;
            }

            set
            {
                if (!string.IsNullOrEmpty(_basepath) && !string.IsNullOrEmpty(value))
                    value = Path.GetFullPath(Path.Combine(_basepath, value));

                _listAlias[Const_AliasName].FileName = value;

                _basepath = Path.GetDirectoryName(_listAlias[Const_AliasName].FileName);
            }
        }

        public static LogLevel LogLevel
        {
            get
            {
                return _listAlias[Const_AliasName].Level;
            }
            set
            {
                _listAlias[Const_AliasName].Level = value;

                foreach (var item in _listAlias.Values)
                {
                    if (item.Level == LogLevel.Default)
                        item.SetLogLevelIfDefault(value);
                }
            }
        }

        public static int FileSize
        {
            get
            {
                return _listAlias[Const_AliasName].FileSize;
            }
            set
            {
                _listAlias[Const_AliasName].FileSize = value;
            }
        }

        public static LogLevel ConvertToLogLevel(string level)
        {
            switch (level.ToLower())
            {
                case "debug":
                    return LogLevel.Debug;
                case "error":
                    return LogLevel.Error;
                case "fatal":
                    return LogLevel.Fatal;
                case "info":
                    return LogLevel.Info;
                case "warning":
                    return LogLevel.Warn;
                default:
                    return LogLevel.Trace;
            }
        }


        internal static void Write(LogLevel level, object mess, string alias = null)
        {

            var sb = new StringBuilder();
            sb.Append(toBaseString(mess));

            writeCore(level, string.Format("{0}", sb), alias);
        }

        private static void write(LogLevel level, object[] messArray, string alias = null)
        {

            var sb = new StringBuilder();
            for (var i = 0; i <= messArray.Length - 1; i++)
            {
                sb.Append(toBaseString(messArray[i]));
                sb.Append(" ");
            }

            writeCore(level, string.Format("{0}", sb), alias);
        }

        /*
        public static void Write(LogLevel level, string message, string alias = null)
        {
            WriteA(level, message, alias);
        }

        public static void Write(Exception ex, string alias = null)
        {
            WriteA(ex, alias);
        }

        public static void Write(Exception ex, string mess, string alias)
        {
            WriteA(ex, mess, alias);
        }


        internal static void WriteA(LogLevel level, string message, string alias = null)
        {
            writeCoreA(level, message, alias);
        }

        internal static void WriteA(Exception ex, string alias = null)
        {
            writeExCore(LogLevel.Fatal, writeException(ex), alias);
        }

        internal static void WriteA(Exception ex, string mess, string alias)
        {

            writeExCore(LogLevel.Fatal, string.Format("{0}{1}", mess, writeException(ex)), alias);
        }
        */

        public static void Fatal(params string[] messArray)
        {
            FatalA(messArray);            
        }

        public static void Error(params string[] messArray)
        {
            ErrorA(messArray);
            
        }

        public static void Warn(params string[] messArray)
        {
            WarnA(messArray);
            
        }

        public static void Info(params string[] messArray)
        {
            InfoA(messArray);
        }

        public static void Trace(params string[] messArray)
        {
            TraceA(messArray);
        }

        public static void Debug(params string[] messArray)
        {
            DebugA(messArray);
        }

        public static void Fatal(params object[] messArray)
        {
            FatalA(messArray);
        }

        public static void Error(params object[] messArray)
        {
            ErrorA(messArray);            
        }

        public static void Warn(params object[] messArray)
        {
            WarnA(messArray);
        }

        public static void Info(params object[] messArray)
        {
            InfoA(messArray);            
        }

        public static void Debug(params object[] messArray)
        {
            DebugA(messArray);            
        }

        public static void Trace(params object[] messArray)
        {
            TraceA(messArray);            
        }
        
        internal static void FatalA(params string[] messArray)
        {
            write(LogLevel.Fatal, messArray, _alias);
        }

        internal static void ErrorA(params string[] messArray)
        {
            write(LogLevel.Error, messArray, _alias);
        }

        internal static void WarnA(params string[] messArray)
        {

            write(LogLevel.Warn, messArray, _alias);
        }

        internal static void InfoA(params string[] messArray)
        {

            write(LogLevel.Info, messArray, _alias);
        }

        internal static void TraceA(params string[] messArray)
        {

            write(LogLevel.Trace, messArray, _alias);
        }

        internal static void DebugA(params string[] messArray)
        {

            write(LogLevel.Debug, messArray, _alias);
        }

        internal static void FatalA(params object[] messArray)
        {
            write(LogLevel.Fatal, messArray, _alias);
        }

        internal static void ErrorA(params object[] messArray)
        {
            write(LogLevel.Error, messArray, _alias);
        }

        internal static void WarnA(params object[] messArray)
        {
            write(LogLevel.Warn, messArray, _alias);
        }

        internal static void InfoA(params object[] messArray)
        {

            write(LogLevel.Info, messArray, _alias);
        }

        internal static void DebugA(params object[] messArray)
        {

            write(LogLevel.Debug, messArray, _alias);
        }

        internal static void TraceA(params object[] messArray)
        {

            write(LogLevel.Trace, messArray, _alias);
        }


        private static void write(LogLevel level, string[] messArray, string alias = null)
        {

            var sb = arrayToMessage(messArray);

            writeCore(level, string.Format("{0}", sb), alias);
        }

        private static void writeExCore(LogLevel level, string mes, string alias)
        {
            writeCore(level, mes, alias, true);
        }

        public static void SetAlias(string alias = null)
        {
            _alias = alias;
        }

        private static string arrayToMessage(object[] messArray)
        {
            var sb = new StringBuilder();
            for (var i = 0; i <= messArray.Length - 1; i++)
            {
                sb.Append(messArray[i] ?? "{(null)}");
                sb.Append(" ");
            }
            return sb.ToString();
        }



        private static string toBaseString(object arg, string prev = "", int maxlevel = 0)
        {
            var str = toBaseStringExt(arg, prev, maxlevel);
            if (str.Contains("\n"))
                return String.Format("{0}{1}", Environment.NewLine, str);
            else
                return str;
        }
        private static string toBaseStringExt(object arg, string prev, int maxlevel)
        {
            if (maxlevel > 9)
                return arg.ToString();

            try
            {
                if (arg == null)
                    return "{(null)}";
                else
                    if (arg is IDictionary)
                {
                    prev += "        ";
                    var dict = new Dictionary<object, object>();
                    foreach (var item in (arg as IDictionary).Keys)
                    {
                        dict[item] = (arg as IDictionary)[item];
                    }

                    var sb = new StringBuilder();
                    foreach (var item in dict)
                    {
                        sb.Append(Environment.NewLine);
                        sb.Append(prev);
                        sb.Append(string.Format("key: {0}, value: {1}", toBaseString(item.Key, prev, maxlevel + 1), toBaseString(item.Value, prev, maxlevel + 1)));
                    }

                    return sb.ToString();
                }
                else if (arg is IEnumerable && !(arg is string))
                    return arg is Byte[] ? BitConverter.ToString((Byte[])arg) : arrayToMessage((arg as IEnumerable), maxlevel);
                else if (arg is DataRow)
                    return getDataFromRow((DataRow)arg);
                else if (arg is DataSet)
                    return getDataFromDataSet((DataSet)arg);
                else if (string.Compare(arg.ToString(), arg.GetType().ToString(), true) == 0)
                    return getLogFor(arg, prev, maxlevel);
            }
            catch
            {
                return arg.ToString();
            }

            return arg.ToString();
        }

        private static string getDataFromDataSet(DataSet arg)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < arg.Tables.Count; i++)
            {
                sb.AppendLine(string.Format("Add table index {0}", i));
                foreach (DataRow item in arg.Tables[i].Rows)
                {
                    sb.AppendLine(getDataFromRow(item));
                }
            }
            return sb.ToString();
        }

        private static string getDataFromRow(DataRow dataRow)
        {
            var table = dataRow.Table;
            var sb = new StringBuilder();
            for (int i = 0; i < table.Columns.Count; i++)
            {
                sb.Append(table.Columns[i].Caption);
                sb.Append(" = ");
                sb.Append(toBaseString(dataRow[i]));
                sb.Append("; ");
            }

            return sb.ToString();
        }

        public static bool LogNonPublic { get; set; }

        private static string getLogFor(object target, string prev, int maxlevel)
        {
            var prView = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
            if (LogNonPublic)
                prView |= BindingFlags.NonPublic;
            var properties =
                from property in target.GetType().GetProperties(prView)
                select new
                {
                    property.Name,
                    Value = property.GetValue(target)
                };

            var builder = new StringBuilder(prev).Append(target.ToString()).AppendLine();
            prev += "       ";
            foreach (var property in properties)
            {
                builder
                    .Append(prev)
                    .Append(property.Name)
                    .Append(" = ")
                    .Append(toBaseString(property.Value, prev, maxlevel + 1))
                    .AppendLine();
            }



            var fields =
                        from field in target.GetType().GetFields(prView)
                        select new
                        {
                            field.Name,
                            Value = field.GetValue(target)
                        };

            foreach (var property in fields)
            {
                builder
                    .Append(prev)
                    .Append(property.Name)
                    .Append(" = ")
                    .Append(toBaseString(property.Value, prev, maxlevel + 1))
                    .AppendLine();
            }


            return builder.ToString();
        }

        private static string arrayToMessage(IEnumerable enumerable, int maxlevel)
        {
            var sb = new StringBuilder();
            sb.Append("Array: ");
            Int64 index = 0;
            foreach (var item in enumerable)
            {
                sb.Append(string.Format("[{0}] = ", index));
                if (item == null)
                {
                    continue;
                }

                sb.Append(toBaseString(item, "", maxlevel + 1));
                sb.Append(" ");
                index++;
            }

            return sb.ToString();
        }



        private static void writeCoreA(LogLevel level, string message, string alias)
        {
            writeCore(level, message, alias);
        }

        private static void writeCore(LogLevel level, string message, string alias, bool isEx = false)
        {


            FileLogData str = null;

            if (!string.IsNullOrEmpty(alias))
            {

                if (!_listAlias.TryGetValue(alias, out str))
                {
                    _alias = null;
                    return;
                }
                _alias = null;
            }
            else str = _listAlias[Const_AliasName];

            str.WriteMessage(level, message);
            _lastmessage[level] = message;


        }




        private static string _alias;
        private static string _basepath;
        private static bool _isConsole;

        private static string writeException(Exception ex)
        {
            if (ex.InnerException != null)
                ex = ex.InnerException;

            var mess = new StringBuilder();

            mess.Append(Environment.NewLine);
            mess.Append(string.Format("Message: {0}", toBaseString(ex.Message)));
            mess.Append(Environment.NewLine);
            mess.Append("\tData:");
            mess.Append(Environment.NewLine);

            foreach (object s in ex.Data)
            {
                mess.Append("     " + toBaseString(s));
                mess.Append(Environment.NewLine);
            }

            mess.Append(string.Format("\tSource: {0}", toBaseString(ex.Source)));
            mess.Append(Environment.NewLine);
            mess.Append(string.Format("\tStackTrace: {0}", toBaseString(ex.StackTrace)));
            mess.Append(Environment.NewLine);
            mess.Append(string.Format("\tTargetSite: {0}", toBaseString(ex.TargetSite)));
            mess.Append(Environment.NewLine);

            return mess.ToString();
        }


    }

}
