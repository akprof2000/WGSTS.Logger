using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading;

namespace WGSTS.Logger
{
    static class NLogHelper
    {
        public static bool IsConsole { get; set; }
        public static NLog.LogLevel ConsoleLevel { get; set; } = NLog.LogLevel.Trace;

        private static ConcurrentDictionary<string, NLog.Logger> _logger = new ConcurrentDictionary<string, NLog.Logger>();


        static NLogHelper()
        {

            IsConsole = Environment.UserInteractive;
            //_logger = LogManager.GetCurrentClassLogger();
            var config = new LoggingConfiguration();


            NLog.LogManager.Configuration = config;



        }

        static internal void FlushData()
        {
            var reset = new ManualResetEventSlim(false);
            NLog.LogManager.Flush(ex => reset.Set(), TimeSpan.FromSeconds(15));
            reset.Wait(TimeSpan.FromSeconds(15));
        }

        static ConcurrentDictionary<string, LoggingRule> _listRule = new ConcurrentDictionary<string, LoggingRule>();

        public static bool Compression { get; set; } = true;

        internal static void GenerateAlias(string fileName, string alias, int filecount, int filesize, int skipframes)
        {
            if (string.IsNullOrEmpty(fileName))
                return;

          

            var ext = Path.GetExtension(fileName);
            var fn = Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName));



            var config = NLog.LogManager.Configuration;


            var verbose = "${date:format=dd.MM HH\\:mm\\:ss.fff} ${uppercase:${level}:padding=-5} ${pad:padding=-90:fixedLength=false:inner=[${threadid}]${callsite:skipFrames=1:className=True:fileName=True:includeNamespace=True:includeSourcePath=False:methodName=True:cleanNamesOfAnonymousDelegates=True}} ${message} ${when:when=level==LogLevel.Fatal:inner=${newline}${stacktrace::skipFrames=1:format=DetailedFlat:separator=\r\n}}";
            verbose = verbose.Replace("skipFrames=1", $"skipFrames={skipframes}");
            var verbose_inline = "${replace:inner=${verbose}:searchFor=\\r\\n|\\n:replaceWith=\r\n                         :regex=true}".Replace("${verbose}", verbose);



            var extext = Compression ? ".zip" : ext;

            var ftarget = new FileTarget()
            {
                FileName = $"{fn}{ext}",
                Layout = verbose_inline,
                MaxArchiveFiles = filecount,
                ArchiveAboveSize = filesize,
                EnableArchiveFileCompression = Compression,
                ArchiveNumbering = ArchiveNumberingMode.Rolling,
                ConcurrentWrites = true,
                ArchiveFileName = $"{fn}.{{##}}{extext}"
            };


            config.RemoveTarget($"asyncFileBaseData{alias}");

            var atw = config.FindTargetByName<AsyncTargetWrapper>($"asyncFileBaseData{alias}");
            if (atw == null)
            {
                atw = new AsyncTargetWrapper()
                {
                    Name = $"asyncFileBaseData{alias}",
                    QueueLimit = 5000,
                    OverflowAction = AsyncTargetWrapperOverflowAction.Block,
                    WrappedTarget = ftarget
                };
            }

            config.AddTarget($"asyncFileBaseData{alias}", atw);

            LoggingRule rule2;

            if (_listRule.ContainsKey($"asyncFileBaseData{alias}"))
            {
                config.LoggingRules.Remove(_listRule[$"asyncFileBaseData{alias}"]);
            }

            if (string.IsNullOrEmpty(alias))
            {

                if (_listRule.ContainsKey("console"))
                {
                    config.LoggingRules.Remove(_listRule["console"]);
                }

                if (IsConsole)
                {
                    var target = new ColoredConsoleTarget()
                    {
                        DetectConsoleAvailable = true,
                        Layout = verbose_inline
                    };

                    config.AddTarget("console", target);
                    var rule1 = new LoggingRule("*", ConsoleLevel, target);
                    config.LoggingRules.Add(rule1);
                    _listRule[$"console"] = rule1;
                }
                
                rule2 = new LoggingRule("*", NLog.LogLevel.Trace, atw);
                config.LoggingRules.Add(rule2);
            }
            else
            {
                rule2 = new LoggingRule(alias, NLog.LogLevel.Trace, atw);
                config.LoggingRules.Add(rule2);

            }
            _listRule[$"asyncFileBaseData{alias}"] = rule2;

            NLog.LogManager.Configuration = config;

            if (string.IsNullOrEmpty(alias))
            {
                _logger[""] = NLog.LogManager.GetCurrentClassLogger();
            }
            else
                _logger[alias] = NLog.LogManager.GetLogger(alias);

            NLog.LogManager.Configuration.Reload();
        }


        internal static void SetLogLevel(NLog.LogLevel level, string alias)
        {
            if (_logger.TryGetValue(alias, out NLog.Logger log))
            {
                alias = alias == "" ? "*" : alias;

                foreach (var item in NLog.LogManager.Configuration.LoggingRules)
                {
                    if (item.Targets.FirstOrDefault() is ColoredConsoleTarget)
                        continue;
                    if (item.NameMatches(alias))
                    {
                        item.DisableLoggingForLevel(NLog.LogLevel.Off);
                        item.DisableLoggingForLevel(NLog.LogLevel.Trace);
                        item.DisableLoggingForLevel(NLog.LogLevel.Debug);
                        item.DisableLoggingForLevel(NLog.LogLevel.Info);
                        item.DisableLoggingForLevel(NLog.LogLevel.Warn);
                        item.DisableLoggingForLevel(NLog.LogLevel.Error);
                        item.DisableLoggingForLevel(NLog.LogLevel.Fatal);
                        item.EnableLoggingForLevels(level, NLog.LogLevel.Off);
                    }
                }
                NLog.LogManager.ReconfigExistingLoggers();

            }
        }

        internal static void Info(object obj, string alias)
        {
            if (_logger.TryGetValue(alias, out NLog.Logger log))
            {
                log?.Info(obj);
            }
        }

        internal static void Trace(object obj, string alias)
        {
            if (_logger.TryGetValue(alias, out NLog.Logger log))
            {
                log?.Trace(obj);
            }
        }


        internal static void Debug(object obj, string alias)
        {

            if (_logger.TryGetValue(alias, out NLog.Logger log))
            {
                log?.Debug(obj);
            }
        }




        internal static void Warn(object obj, string alias)
        {

            if (_logger.TryGetValue(alias, out NLog.Logger log))
            {
                log?.Warn(obj);
            }
        }



        internal static void Error(object obj, string alias)
        {

            if (_logger.TryGetValue(alias, out NLog.Logger log))
            {
                log?.Error(obj);
            }
        }

        internal static void Fatal(object obj, string alias)
        {
            if (_logger.TryGetValue(alias, out NLog.Logger log))
            {
                log?.Fatal(obj);
            }
        }

    }

}
