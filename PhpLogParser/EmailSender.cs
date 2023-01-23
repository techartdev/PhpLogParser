using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PhpLogParser
{
    public static class EmailSender
    {
        public static void SendEmail(string pdfFilePath)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("youremail@gmail.com");
            mail.To.Add("info@example.com");
            mail.Subject = "Log report";
            mail.Body = "Please find attached the log report file";

            // Add the PDF file as an attachment
            Attachment attachment = new Attachment(pdfFilePath);
            mail.Attachments.Add(attachment);

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new NetworkCredential("youremail@gmail.com", "password");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
        }
    }
}
