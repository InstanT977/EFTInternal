using BehaviourMachine;
using NLog;
using System;
using System.IO;
using UnityEngine;

namespace EscapeFromGod.Functions
{
    class Logs
    {
        private static string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private static string bootUpTimestamp;

        private static string GetTimeStamp(bool day)
        {
            if (day)
                return DateTime.Now.ToString("dd.MM.yyyy_HH.mm.ss");
            else
                return DateTime.Now.ToString("HH:mm:ss");
        }

        public static void Startup()
        {
            bootUpTimestamp = GetTimeStamp(true);
        }

        #region logLevel Declaration
        public enum logLevelEnum
        {
            Low,
            High,
            Critical,
            Medium,
            Debug
        }
        public static string logLevel(logLevelEnum LogLevel)
        {
            switch (LogLevel)
            {
                case logLevelEnum.Low:
                    return "[Low]";

                case logLevelEnum.High:
                    return "[High]";

                case logLevelEnum.Critical:
                    return "[CRITICAL]";
                case logLevelEnum.Debug:
                    return "[DEBUG]";

                default:
                    return "";
            }
        }
        #endregion

        public static bool Add(string log, logLevelEnum LogLevel)
        {
            using (TextWriter outputFile = new StreamWriter(Path.Combine(docPath, "EscapeFromNoName/logs_" + bootUpTimestamp + ".txt"), true))
            {
                try
                {
                    outputFile.WriteLine(string.Format("[{0}]{1} : {2};", GetTimeStamp(false), logLevel(LogLevel), log));
                    return true;
                }
                catch { }
                return false;
            }
        }
    }
}
