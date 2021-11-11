﻿using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using ImperaPlus.Application;
using Typesafe.Mailgun;
using NLog.Fluent;

namespace ImperaPlus.Web.Services
{
    public class MailGunSettings
    {
        public string Domain { get; set; }

        public string ApiKey { get; set; }
    }

    public class MailGunEmailService : IEmailService
    {
        private MailGunSettings settings;

        public MailGunEmailService(MailGunSettings settings)
        {
            Log.Debug().Message("Starting MailGunEmailService {0} {1}", settings.Domain, settings.ApiKey).Write();

            this.settings = settings;
        }

        public Task SendMail(string to, string subject, string bodyText)
        {
            return SendMail(to, subject, bodyText, bodyText);
        }

        public Task SendMail(string to, string subject, string bodyHtml, string bodyText)
        {
            var client = new MailgunClient(settings.Domain, settings.ApiKey, 3);

            var myMessage = new MailMessage();
            myMessage.To.Add(to);

            myMessage.From = new MailAddress("info@imperaonline.de", "Impera Team");
            myMessage.Subject = subject;

            myMessage.AlternateViews.Add(
                AlternateView.CreateAlternateViewFromString(bodyText, null, MediaTypeNames.Text.Plain));
            myMessage.AlternateViews.Add(
                AlternateView.CreateAlternateViewFromString(bodyHtml, null, MediaTypeNames.Text.Html));

            client.SendMail(myMessage);

            return Task.FromResult(0);
        }
    }
}
