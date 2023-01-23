using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhpLogParser
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = "website.log";

            ReportGenerator.GenerateReports(filePath, "report.html");

            PdfReportGenerator.GenerateReports(filePath, "report.pdf");

            EmailSender.SendEmail("report.pdf");
        }
    }
}
