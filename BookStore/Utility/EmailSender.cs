using Microsoft.AspNetCore.Identity.UI.Services;

using System.Threading.Tasks;

namespace BookStore.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }
        public async Task Execute(string email, string subject, string body) 
        {
            //mailjet.api
        }
    }
}
