using System;
using System.Threading.Tasks;

namespace Celestial.EmailService.Services
{
    public interface IEmailService
    {
        Task SendEmailAddressConfirmationLink(Guid userId, string userEmail, string userName, string token);
		void SendEmail(string email, string subject, string htmlMessage);
	}
}
