using System.Threading.Tasks;
using ImperaPlus.Application;

namespace ImperaPlus.TestSupport
{
    public class FakeEmailService : IEmailService
    {
        public Task SendMail(string to, string subject, string bodyText)
        {
            return Task.FromResult(0);
        }

        public Task SendMail(string to, string subject, string bodyHtml, string bodyText)
        {
            return Task.FromResult(0);
        }
    }
}
