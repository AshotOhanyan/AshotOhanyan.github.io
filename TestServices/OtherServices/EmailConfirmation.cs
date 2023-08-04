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
        public static async Task SendEmail(string recipient, string subject, string body)
        {

            SmtpClient smtpClient = new SmtpClient();
            MailMessage message = new MailMessage();


            Task ConfigureSmtpClient = Task.Factory.StartNew(() => 
            {
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(CompanyInfo.CompanyEmail, CompanyInfo.CompanyPassword);
                smtpClient.EnableSsl = true;
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;
            });

            Task<MailMessage> ConfigureMailMessage = new Task<MailMessage>(() =>
            {
                message.From = new MailAddress(CompanyInfo.CompanyEmail);
                message.To.Add(recipient);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = false;

                return message;
            });  
            ConfigureMailMessage.Start();

            await Task.WhenAll(ConfigureSmtpClient,ConfigureMailMessage);

            smtpClient.Send(message);
        }
    }
}
