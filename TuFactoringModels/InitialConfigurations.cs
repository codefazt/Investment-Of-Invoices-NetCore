using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TuFactoringModels
{
    public class InitialConfigurations
    {
        private static int Time { get; set; }
        private static string Port { get; set; }

        public static int GetTimeSesion()
        {
            return Time;
        }

        public static string GetPort()
        {
            return Port;
        }

        public static void SetTimeSesion(int timeOff)
        {
            var myJsonString = File.ReadAllText("appsettings.json");
            dynamic jToken = JToken.Parse(myJsonString);

            Time = jToken.Timeout;
        }

        public static void SetPort()
        {
            var myJsonString = File.ReadAllText("appsettings.json");
            dynamic jToken = JToken.Parse(myJsonString);

            Port = jToken.Port;
        }
    }

    public class TimeSesion
    {
        public int Timeout { get; set; }
    }
}
