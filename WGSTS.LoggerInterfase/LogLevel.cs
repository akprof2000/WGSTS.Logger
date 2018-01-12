using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WGSTS.LoggerInterfase
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum LogLevel
    {
        Trace = 5,
        Debug = 4,
        Info = 3,
        Warn = 2,
        Error = 1,
        Fatal = 0,
        Off = -1,
        Default = int.MaxValue
    }
}
