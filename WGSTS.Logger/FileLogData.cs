using System;
using System.IO;

namespace WGSTS.Logger
{
    class FileLogData
    {
        private const string BaseTrace = @"Logging";
        private string File_Name = Path.Combine(BaseTrace, "log.log");


        private string _fileFullName;
        public string FileName
        {
            get
            {
                return _fileFullName;
            }

            set
            {
                var older = _fileFullName;
                try
                {
                    var baseregpath = AppDomain.CurrentDomain.BaseDirectory;

                   _fileFullName = Path.Combine(baseregpath, BaseTrace, value);


                }
                catch
                {
                    _fileFullName = value;
                }


                try
                {
                    _fileFullName = Path.ChangeExtension(_fileFullName, ".log");
                    _fileFullName = Path.GetFullPath(_fileFullName);
                }
                catch
                {
                    _fileFullName = value;
                }


                var dir = Path.GetDirectoryName(_fileFullName);
                if (!Directory.Exists(dir))
                {
                    try
                    {
                        Directory.CreateDirectory(dir);
                    }
                    catch
                    {
                        _fileFullName = File_Name;
                    }
                }

                if (string.Compare(older, _fileFullName, true) != 0)
                {
                    reinitLogger();

                }
            }
        }

        public void Init()
        {
            _isActive = true;
            reinitLogger();
        }

        private void reinitLogger()
        {
            if (!_isActive)
                return;


            NLogHelper.GenerateAlias(_fileFullName, string.Compare("AUTO -1", Name, true) == 0 ? "" : Name, FileCount, FileSize, DeepParse);
            changeLogLevel();
        }

        int _filesize = 1024 * 1024 * 10;
        internal int FileSize
        {
            get { return _filesize; }
            set
            {
                if (_filesize != value)
                {
                    _filesize = value;
                    reinitLogger();
                }
            }
        }

        internal void SetLogLevelIfDefault(LogLevel level = LogLevel.Debug)
        {
            if (_sublevel == level)
                return;

            _sublevel = level;
            changeLogLevel();
        }

        private void changeLogLevel()
        {
            var loglv = NLog.LogLevel.Trace;
            switch (Level)
            {
                case LogLevel.Trace:
                    loglv = NLog.LogLevel.Trace;
                    break;
                case LogLevel.Debug:
                    loglv = NLog.LogLevel.Debug;
                    break;
                case LogLevel.Info:
                    loglv = NLog.LogLevel.Info;
                    break;
                case LogLevel.Warn:
                    loglv = NLog.LogLevel.Warn;
                    break;
                case LogLevel.Error:
                    loglv = NLog.LogLevel.Error;
                    break;
                case LogLevel.Fatal:
                    loglv = NLog.LogLevel.Fatal;
                    break;
                case LogLevel.Off:
                    loglv = NLog.LogLevel.Off;
                    break;
                case LogLevel.Default:
                    switch (_sublevel)
                    {
                        case LogLevel.Default:
                        case LogLevel.Trace:
                            loglv = NLog.LogLevel.Trace;
                            break;
                        case LogLevel.Debug:
                            loglv = NLog.LogLevel.Debug;
                            break;
                        case LogLevel.Info:
                            loglv = NLog.LogLevel.Info;
                            break;
                        case LogLevel.Warn:
                            loglv = NLog.LogLevel.Warn;
                            break;
                        case LogLevel.Error:
                            loglv = NLog.LogLevel.Error;
                            break;
                        case LogLevel.Fatal:
                            loglv = NLog.LogLevel.Fatal;
                            break;
                        case LogLevel.Off:
                            loglv = NLog.LogLevel.Off;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }

            NLogHelper.SetLogLevel(loglv, string.Compare("AUTO -1", Name, true) == 0 ? "" : Name);
        }

        int _deepParse = 1;
        internal int DeepParse
        {
            get { return _deepParse; }
            set
            {
                if (_deepParse != value)
                {
                    _deepParse = value;
                    reinitLogger();
                }
            }
        }
        int _fileCount = 20;
        internal int FileCount
        {
            get { return _fileCount; }
            set
            {
                if (_fileCount != value)
                {
                    _fileCount = value;
                    reinitLogger();
                }
            }
        }



        LogLevel _level = LogLevel.Trace;
        internal LogLevel Level
        {
            get { return _level; }
            set
            {
                if (_level != value)
                {
                    _level = value;
                    changeLogLevel();
                }
            }
        }

        string _name = "AUTO -1";

        private LogLevel _sublevel = LogLevel.Trace;
        private bool _isActive;

        internal string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    reinitLogger();
                }
            }
        }

        internal void WriteMessage(LogLevel level, string message)
        {
            var name = string.Compare("AUTO -1", Name, true) == 0 ? "" : Name;
            switch (level)
            {
                case LogLevel.Default:
                case LogLevel.Trace:
                    NLogHelper.Trace(message, name);
                    break;
                case LogLevel.Debug:
                    NLogHelper.Debug(message, name);
                    break;
                case LogLevel.Info:
                    NLogHelper.Info(message, name);
                    break;
                case LogLevel.Warn:
                    NLogHelper.Warn(message, name);
                    break;
                case LogLevel.Error:
                    NLogHelper.Error(message, name);
                    break;
                case LogLevel.Fatal:
                    NLogHelper.Fatal(message, name);
                    break;
                case LogLevel.Off:
                    break;

                default:
                    break;
            }
        }
    }

}
