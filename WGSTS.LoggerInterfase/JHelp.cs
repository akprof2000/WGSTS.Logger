﻿using Newtonsoft.Json;
using System;
using System.IO;

namespace WGSTS.Logger
{
    public static class JHelp
    {

        public static object FromJson(this string value, Type tp)
        {
            try
            {
                return JsonConvert.DeserializeObject(value, tp);
            }
            catch
            {
                return null;
            }
        }


        public static T FromJson<T>(this string value)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            catch
            {
                return default(T);
            }
        }


        public static object FromJsonAuto(this string value, Type tp)
        {
            var settings = new JsonSerializerSettings()
            {              
                TypeNameHandling = TypeNameHandling.Auto
            };
            try
            {
                return JsonConvert.DeserializeObject(value, tp, settings);
            }
            catch
            {
                return null;
            }
        }


        public static T FromJsonAuto<T>(this string value)
        {
            var settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto
            };

            try
            {
                return JsonConvert.DeserializeObject<T>(value, settings);
            }
            catch
            {
                return default(T);
            }
        }


        public static object FromJsonAll(this string value, Type tp)
        {
            var settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            };
            try
            {
                return JsonConvert.DeserializeObject(value, tp, settings);
            }
            catch
            {
                return null;
            }
        }


        public static T FromJsonAll<T>(this string value)
        {
            var settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            };

            try
            {
                return JsonConvert.DeserializeObject<T>(value, settings);
            }
            catch
            {
                return default(T);
            }
        }

        public static string ToJson(this object value)
        {
            return ToJson(value, false);
        }


        public static string ToJson(this object value, bool formating)
        {
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            try
            {
                return JsonConvert.SerializeObject(value, formating ? Formatting.Indented : Formatting.None, settings);
            }
            catch
            {
                return null;
            }
        }

        public static string ToJsonAuto(this object value)
        {
            return ToJsonAuto(value, false);
        }


        public static string ToJsonAuto(this object value, bool formating)
        {
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling =  ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Auto                
            };
            try
            {
                return JsonConvert.SerializeObject(value, formating ? Formatting.Indented : Formatting.None, settings);
            }
            catch
            {
                return null;
            }
        }

        public static string ToJsonAll(this object value)
        {
            return ToJsonAll(value, false);
        }


        public static string ToJsonAll(this object value, bool formating)
        {
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.All
            };
            try
            {
                return JsonConvert.SerializeObject(value, formating ? Formatting.Indented : Formatting.None, settings);
            }
            catch
            {
                return null;
            }
        }

        public static string MemoryStreamToJson(this MemoryStream value)
        {
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            try
            {
                var bytes = value.ToArray();
                return JsonConvert.SerializeObject(bytes, Formatting.Indented, settings);
            }
            catch
            {
                return null;
            }
        }

        public static object MemoryStreamFromJson(this string value)
        {
            try
            {
                return JsonConvert.DeserializeObject<byte[]>(value);
            }
            catch
            {
                return null;
            }
        }

    }

}
