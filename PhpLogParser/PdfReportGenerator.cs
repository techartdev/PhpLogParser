using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PhpLogParser
{
    public static class PdfReportGenerator
    {
        public static void GenerateReports(string logFile, string reportPath)
        {
            LogEntry[] logEntries = LogParser.ParseLogFile(logFile);

            // Create a new PDF document
            PdfDocument pdf = new PdfDocument();

            // Add a page for Top 10 IP addresses report
            PdfPage pdfPage = pdf.AddPage();
            var top10IPs = logEntries
                .GroupBy(x => x.IP)
                .Select(x => new { IP = x.Key, Count = x.Count() })
                .OrderByDescending(x => x.Count)
                .Take(10);

            GenerateTable(pdfPage, top10IPs, "Top 10 IP addresses");

            // Add a page for Top 10 Requested URLs report
            pdfPage = pdf.AddPage();

            var top10Urls = logEntries
                .GroupBy(x => x.Request)
                .Select(x => new { Url = x.Key, Count = x.Count() })
                .OrderByDescending(x => x.Count)
                .Take(10);
            GenerateTable(pdfPage, top10Urls, "Top 10 Requested URLs");

            // Add a page for Top 10 URLs for each of the Top 10 IPs report
            pdfPage = pdf.AddPage();

            var top10UrlsPerIP = logEntries
                .GroupBy(x => x.IP)
                .Select(g => new
                {
                    IP = g.Key,
                    URLs = g
                        .GroupBy(x => x.Request)
                        .Select(x => new { Url = x.Key, Count = x.Count() })
                        .OrderByDescending(x => x.Count)
                        .Take(10)
                });

            foreach (var ip in top10UrlsPerIP)
            {
                XGraphics graph = XGraphics.FromPdfPage(pdfPage);
                XFont font = new XFont("Verdana", 10, XFontStyle.Regular);
                int y = 50;
                graph.DrawString("Top 10 URLs requested by IP: " + ip.IP, font, XBrushes.Black,
                    new XRect(50, y, pdfPage.Width.Point - 100, 0), XStringFormats.TopLeft);
                y += 20;
                graph.Dispose();
                GenerateTable(pdfPage, ip.URLs, "");
                pdfPage = pdf.AddPage();
            }

            // Save the document...
            pdf.Save(reportPath);
        }



        private static void GenerateTable(PdfPage pdfPage, IEnumerable data, string title)
        {
            XGraphics graph = XGraphics.FromPdfPage(pdfPage);
            XFont font = new XFont("Verdana", 10, XFontStyle.Regular);
            int y = 50;

            if (!string.IsNullOrEmpty(title))
            {
                graph.DrawString(title, font, XBrushes.Black, new XRect(50, y, pdfPage.Width.Point - 100, 20), XStringFormats.TopLeft);
                y += 20;
            }

            int x = 50;
            int rowHeight = 20;
            int columnWidth = (int)((pdfPage.Width.Point - 100) / data.Cast<dynamic>().First().GetType().GetProperties().Length);

            // Draw table headers
            foreach (PropertyInfo property in data.Cast<dynamic>().First().GetType().GetProperties())
            {
                graph.DrawRectangle(XPens.Black, XBrushes.LightGray, new XRect(x, y, columnWidth, rowHeight));
                graph.DrawString(property.Name, font, XBrushes.Black, new XRect(x, y, columnWidth, rowHeight), XStringFormats.Center);
                x += columnWidth;
            }

            y += rowHeight;

            // Draw table rows
            foreach (var item in data)
            {
                x = 50;
                foreach (PropertyInfo property in item.GetType().GetProperties())
                {
                    graph.DrawRectangle(XPens.Black, XBrushes.White, new XRect(x, y, columnWidth, rowHeight));
                    graph.DrawString(property.GetValue(item).ToString(), font, XBrushes.Black, new XRect(x, y, columnWidth, rowHeight), XStringFormats.Center);
                    x += columnWidth;
                }

                y += rowHeight;
            }
        }



    }
}
