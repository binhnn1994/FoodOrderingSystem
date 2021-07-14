using FoodOrderingSystem.Utils;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Services.Interfaces
{
    public interface ISendMailService
    {
        Task SendMail(MailContent mailContent);

        Task SendEmailAsync(string email, string subject, string message);
    }
}