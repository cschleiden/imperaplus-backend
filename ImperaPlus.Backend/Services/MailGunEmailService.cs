using System.Configuration;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using ImperaPlus.Application;
using Typesafe.Mailgun;

namespace ImperaPlus.Backend.Services
{
    public class MailGunEmailService : IEmailService
    {
        public Task SendMail(string to, string subject, string bodyHtml, string bodyText)
        {
            var client = new MailgunClient(ConfigurationManager.AppSettings["MailGunDomain"], ConfigurationManager.AppSettings["MailGunApiKey"]);

            MailMessage myMessage = new MailMessage();
            myMessage.To.Add(to);

            myMessage.From = new MailAddress("info@imperaonline.de", "Impera Team");
            myMessage.Subject = subject;

            myMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(bodyText, null, MediaTypeNames.Text.Plain));
            myMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(bodyHtml, null, MediaTypeNames.Text.Html));

            client.SendMail(myMessage);

            return Task.FromResult(0);
        }
    }
}