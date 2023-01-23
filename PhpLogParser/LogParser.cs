using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PhpLogParser
{
    class LogParser
    {
        public static LogEntry[] ParseLogFile(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            Regex logLineRegex = new Regex(@"(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}) - - \[(.+)\] ""(.+)"" (\d+) (\d*|-) ""?(.*)"" ""(.+)""");
            List<LogEntry> logEntries = new List<LogEntry>();
            foreach (var line in lines)
            {
                var match = logLineRegex.Match(line);
                if (match.Success)
                {
                    var log = new LogEntry
                    {
                        IP = match.Groups[1].Value,
                        Date = DateTime.ParseExact(match.Groups[2].Value, "dd/MMM/yyyy:HH:mm:ss zzz", System.Globalization.CultureInfo.InvariantCulture),
                        Request = match.Groups[3].Value,
                        StatusCode = int.Parse(match.Groups[4].Value),
                        ResponseLength = match.Groups[5].Value == "-" ? 0 : int.Parse(match.Groups[5].Value),
                        Referer = match.Groups[6].Value == "" ? "" : match.Groups[6].Value,
                        UserAgent = match.Groups[7].Value
                    };
                    logEntries.Add(log);
                }
            }
            return logEntries.ToArray();
        }
    }



    public class LogEntry
    {
        public string IP { get; set; }
        public DateTime Date { get; set; }
        public string Request { get; set; }
        public int StatusCode { get; set; }
        public int ResponseLength { get; set; }
        public string Referer { get; set; }
        public string UserAgent { get; set; }
    }
}