using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MimeKit;
using NutritionApp.Domain.Interfaces;

namespace NutritionApp.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;
        public EmailSender(IConfiguration config) => _config = config;

        public async Task SendEmailAsync(string to, string subject, string htmlMessage)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config["Smtp:From"]));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlMessage };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            await smtp.ConnectAsync(_config["Smtp:Host"], int.Parse(_config["Smtp:Port"]), MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config["Smtp:User"], _config["Smtp:Pass"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }


    }
}