using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TestServices.ServiceConstants;

namespace TestServices.OtherServices
{
    public static class EmailConfirmation
    {
        public static void SendEmail(string recipient,string subject,string body)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(CompanyInfo.CompanyEmail);
            message.To.Add(recipient);
            message.Subject = subject;  
            message.Body = body;
            message.IsBodyHtml = false;

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(CompanyInfo.CompanyEmail, CompanyInfo.CompanyPassword);
            smtpClient.EnableSsl = true;
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            


            smtpClient.Send(message);
        }
    }
}
