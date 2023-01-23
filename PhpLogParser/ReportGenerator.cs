using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhpLogParser
{
    public static class ReportGenerator
    {
        public static void GenerateReports(string filePath, string reportPath)
        {
            LogEntry[] logEntries = LogParser.ParseLogFile(filePath);

            var top10IPsReport = GenerateTop10IPsReport(logEntries);
            var top10UrlsReport = GenerateTop10UrlsReport(logEntries);
            var top10UrlsPerIPReport = GenerateTop10UrlsPerIPReport(logEntries);

            StringBuilder sb = new StringBuilder();
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("<title>Log Report</title>");
            sb.Append("</head>");
            sb.Append("<body>");
            sb.Append("<h1>Log Report</h1>");
            sb.Append("<h2>Top 10 IPs</h2>");
            sb.Append(top10IPsReport);
            sb.Append("<h2>Top 10 URLs</h2>");
            sb.Append(top10UrlsReport);
            sb.Append("<h2>Top 10 URLs per IP</h2>");
            sb.Append(top10UrlsPerIPReport);
            sb.Append("</body>");
            sb.Append("</html>");

            File.WriteAllText(reportPath, sb.ToString());
        }

        public static string GenerateTop10IPsReport(LogEntry[] logEntries)
        {
            var top10IPs = logEntries
                .GroupBy(x => x.IP)
                .OrderByDescending(x => x.Count())
                .Take(10)
                .Select(x => new { IP = x.Key, Count = x.Count() });

            StringBuilder sb = new StringBuilder();
            sb.Append("<table>");
            sb.Append("<tr><th>IP</th><th>Count</th></tr>");
            foreach (var item in top10IPs)
            {
                sb.Append("<tr>");
                sb.Append($"<td>{item.IP}</td>");
                sb.Append($"<td>{item.Count}</td>");
                sb.Append("</tr>");
            }
            sb.Append("</table>");
            return sb.ToString();
        }

        public static string GenerateTop10UrlsReport(LogEntry[] logEntries)
        {
            var top10Urls = logEntries
                .GroupBy(x => x.Request)
                .OrderByDescending(x => x.Count())
                .Take(10)
                .Select(x => new { URL = x.Key, Count = x.Count() });

            StringBuilder sb = new StringBuilder();
            sb.Append("<table>");
            sb.Append("<tr><th>URL</th><th>Count</th></tr>");
            foreach (var item in top10Urls)
            {
                sb.Append("<tr>");
                sb.Append($"<td>{item.URL}</td>");
                sb.Append($"<td>{item.Count}</td>");
                sb.Append("</tr>");
            }
            sb.Append("</table>");
            return sb.ToString();
        }

        static string GenerateTop10UrlsPerIPReport(LogEntry[] logEntries)
        {
            var top10IPs = logEntries
                .GroupBy(x => x.IP)
                .OrderByDescending(x => x.Count())
                .Take(10);

            StringBuilder sb = new StringBuilder();
            foreach (var ip in top10IPs)
            {
                var top10UrlsPerIP = logEntries
                    .Where(x => x.IP == ip.Key)
                    .GroupBy(x => x.Request)
                    .OrderByDescending(x => x.Count())
                    .Take(10)
                    .Select(x => new { URL = x.Key, Count = x.Count() });

                sb.Append($"<h2>Top 10 URLs for IP: {ip.Key}</h2>");
                sb.Append("<table>");
                sb.Append("<tr><th>URL</th><th>Count</th></tr>");
                foreach (var url in top10UrlsPerIP)
                {
                    sb.Append("<tr>");
                    sb.Append($"<td>{url.URL}</td>");
                    sb.Append($"<td>{url.Count}</td>");
                    sb.Append("</tr>");
                }
                sb.Append("</table>");
            }
            return sb.ToString();
        }


    }
}
