using System;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using ImperaPlus.Application;

namespace ImperaPlus.Web.Services
{
    public class LocalEmailService : IEmailService
    {
        public Task SendMail(string to, string subject, string bodyText)
        {
            return this.SendMail(to, subject, bodyText, bodyText);
        }

        public async Task SendMail(string to, string subject, string bodyHtml, string bodyText)
        {
            MailMessage myMessage = new MailMessage();
            myMessage.To.Add(to);

            myMessage.From = new System.Net.Mail.MailAddress("info@imperaonline.de", "Impera Team");
            myMessage.Subject = subject;

            myMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(bodyText, null, MediaTypeNames.Text.Plain));
            myMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(bodyHtml, null, MediaTypeNames.Text.Html));

            SmtpClient smtpClient = new SmtpClient("localhost", Convert.ToInt32(25));
            await smtpClient.SendMailAsync(myMessage);
        }
    }
}