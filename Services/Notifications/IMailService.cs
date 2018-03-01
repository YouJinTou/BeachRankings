using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Notifications
{
    public interface IMailService
    {
        Task SendMailAsync(
            IEnumerable<string> recipients, string from, string subject, string body);
    }
}
