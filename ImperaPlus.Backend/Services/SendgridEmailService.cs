using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using SendGrid;
using System.Net;
using System.Configuration;
using ImperaPlus.Application;

namespace ImperaPlus.Backend
{
    public class SendgridEmailService : IEmailService
    {       
        public Task SendMail(string to, string subject, string bodyHtml, string bodyText)
        {
            var myMessage = new SendGridMessage();
            myMessage.AddTo(to);
            
            myMessage.From = new System.Net.Mail.MailAddress("info@imperaonline.de", "Impera Team");
            myMessage.Subject = subject;
            myMessage.Text = bodyText;
            myMessage.Html = bodyHtml;

            var credentials = new NetworkCredential(
                       ConfigurationManager.AppSettings["SendGridAccount"],
                       ConfigurationManager.AppSettings["SendGridPassword"]
                       );

            // Create a Web transport for sending email.
            var transportWeb = new Web(credentials);

            // Send the email.
            if (transportWeb != null)
            {
                return transportWeb.DeliverAsync(myMessage);
            }
            else
            {
                // TODO: CS: Log error
                return Task.FromResult(0);
            }
        }
    }       

    public class IdentityMessageService : IIdentityMessageService
    {
        private IEmailService emailService;

        public IdentityMessageService(IEmailService emailService)
        {
            this.emailService = emailService;
        }

        public Task SendAsync(IdentityMessage message)
        {            
            return this.emailService.SendMail(message.Destination, message.Subject, message.Body, message.Body);
        }
    }
}