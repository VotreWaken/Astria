using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Configuration;
using System.Text.Encodings.Web;

namespace Celestial.EmailService.Services
{
    public class DemoEmailService : IEmailService
    {
        private readonly ILogger<DemoEmailService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public IConfiguration _configuration { get; }

        public DemoEmailService(ILogger<DemoEmailService> logger, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public void SendEmail(string email, string subject, string htmlMessage)
        {
            using (MailMessage mm = new MailMessage(_configuration["NetMail:sender"], email))
            {
                mm.Subject = subject;
                mm.Body = htmlMessage;
                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = _configuration["NetMail:smtpHost"];
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential(_configuration["NetMail:sender"], _configuration["NetMail:senderpassword"]);
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);
            }
        }
        public Task SendEmailAddressConfirmationLink(Guid userId, string userEmail, string userFullName, string token)
        {
            string encodedToken = HttpUtility.UrlEncode(token);
            string verificationUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/authentication/confirmEmail?userid={userId}&token={encodedToken}";

            string subject = "Confirm Your Email";

            string htmlMessage = $@"
    <html>
    <head>
        <style>
            body {{
                font-family: Arial, sans-serif;
                background-color: #f4f4f7;
                color: #51545e;
                margin: 0;
                padding: 0;
                -webkit-text-size-adjust: none;
            }}
            .email-wrapper {{
                width: 100%;
                background-color: #f4f4f7;
                padding: 20px;
            }}
            .email-content {{
                max-width: 600px;
                background-color: #ffffff;
                margin: 0 auto;
                padding: 20px;
                border-radius: 10px;
                box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
            }}
            .email-header {{
                text-align: center;
                padding-bottom: 20px;
            }}
            .email-header h1 {{
                font-size: 24px;
                margin: 0;
                color: #333333;
            }}
            .email-body {{
                padding: 20px;
                font-size: 16px;
                line-height: 1.6;
                color: #51545e;
            }}
            .email-body p {{
                margin: 0 0 20px;
            }}
            .email-body a {{
                color: #3869d4 !important;
                text-decoration: none !important;
                font-weight: bold !important;
            }}
            .email-footer {{
                text-align: center;
                font-size: 12px;
                color: #888888;
                margin-top: 20px;
            }}
            .email-button-container {{
                text-align: center;
                margin-top: 30px;
            }}
            .email-button {{
                display: inline-block;
                padding: 10px 20px;
                font-size: 16px;
                color: #ffffff !important;
                background-color: #3869d4;
                border-radius: 5px;
                text-decoration: none;
                font-weight: bold;
            }}
            .email-button:hover {{
                background-color: #2b53a6;
            }}
        </style>
    </head>
    <body>
        <div class='email-wrapper'>
            <div class='email-content'>
                <div class='email-header'>
                    <h1>Welcome, {userFullName}!</h1>
                </div>
                <div class='email-body'>
                    <p>We're excited to have you get started. First, you need to confirm your account.</p>
                    <p>Please click the button below to verify your email address:</p>
                </div>
                <div class='email-button-container'>
                    <a href='{HtmlEncoder.Default.Encode(verificationUrl)}' class='email-button'>Confirm Email</a>
                </div>
                <div class='email-body'>
                    <p>If the button doesn't work, copy and paste the following link into your browser:</p>
                    <p><a href='{HtmlEncoder.Default.Encode(verificationUrl)}'>{verificationUrl}</a></p>
                    <p>If you didn’t create an account using this email address, please ignore this email.</p>
                </div>
            </div>
            <div class='email-footer'>
                <p>&copy; 2024 CelestialRTP. All rights reserved.</p>
            </div>
        </div>
    </body>
    </html>";

            _logger.LogInformation($"Service would send email containing the following verification url: {verificationUrl}");
            SendEmail(userEmail, subject, htmlMessage);
            return Task.CompletedTask;
        }
    }
}