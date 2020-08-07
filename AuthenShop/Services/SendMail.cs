using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AuthenShop.Services
{
    public class SendMail : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient("smtp.gmail.com")
            {
                UseDefaultCredentials = false,
                EnableSsl = true,
                Port = 587,
                Credentials = new NetworkCredential("votantai4899@gmail.com", "Tai01639505022")
            };
            var mailMessage = new MailMessage
            {
                From = new MailAddress("JangleeShop@gmail.com")
            };
            mailMessage.To.Add(email);
            mailMessage.Subject = subject;
            mailMessage.Body = htmlMessage;
            mailMessage.IsBodyHtml = true;
            return client.SendMailAsync(mailMessage);
        }
    }
}