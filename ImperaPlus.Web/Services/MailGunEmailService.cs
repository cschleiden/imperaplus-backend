using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using ImperaPlus.Application;
using Typesafe.Mailgun;

namespace ImperaPlus.Web.Services
{
    public class MailGunEmailService : IEmailService
    {
        public Task SendMail(string to, string subject, string bodyText)
        {
            return this.SendMail(to, subject, bodyText, bodyText);
        }

        public Task SendMail(string to, string subject, string bodyHtml, string bodyText)
        {
            var client = new MailgunClient("", "", 1); // Configuration["MailGunDomain"], Configuration["MailGunApiKey"]);

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