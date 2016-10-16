using System.Threading.Tasks;

namespace ImperaPlus.Application
{
    public interface IEmailService
    {
        Task SendMail(string to, string subject, string bodyText);

        Task SendMail(string to, string subject, string bodyHtml, string bodyText);
    }
}
