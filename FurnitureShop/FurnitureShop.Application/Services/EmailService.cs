using FurnitureShop.Application.Common;
using FurnitureShop.Application.Interfaces.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace FurnitureShop.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendWelcomeEmailAsync(
            string email,
            string fullName)
        {
            var subject = "Welcome to Luxe Living";

            var body = $@"
<h2>Welcome to Luxe Living</h2>

<p>Hello <b>{fullName}</b>,</p>

<p>
Thank you for creating your account.
We are excited to have you with us.
</p>

<p>
Start exploring premium furniture designed to transform your home.
</p>

<p>
Regards,<br/>
<b>Luxe Living Team</b>
</p>";

            await SendEmailAsync(email, subject, body);
        }

        public async Task SendEmailVerificationAsync(
            string email,
            string fullName,
            string verificationToken)
        {
            var verificationLink =
                $"https://yourfrontend.com/verify-email?token={verificationToken}";

            var subject = "Verify your email";

            var body = $@"
<h2>Email Verification</h2>

<p>Hello <b>{fullName}</b>,</p>

<p>
Click the button below to verify your account.
</p>

<p>
<a href='{verificationLink}'
style='background:#000;
padding:12px 20px;
color:white;
text-decoration:none;
border-radius:5px'>
Verify Email
</a>
</p>

<p>
If you didn't create this account,
please ignore this email.
</p>";

            await SendEmailAsync(email, subject, body);
        }

        public async Task SendPasswordResetEmailAsync(
            string email,
            string fullName,
            string resetToken)
        {
            var resetLink =
                $"https://yourfrontend.com/reset-password?token={resetToken}";

            var subject = "Reset your password";

            var body = $@"
<h2>Password Reset</h2>

<p>Hello <b>{fullName}</b>,</p>

<p>
You requested a password reset.
</p>

<p>
<a href='{resetLink}'
style='background:#000;
padding:12px 20px;
color:white;
text-decoration:none;
border-radius:5px'>
Reset Password
</a>
</p>

<p>
If you didn't request this,
please ignore this email.
</p>";

            await SendEmailAsync(email, subject, body);
        }

        private async Task SendEmailAsync(
            string email,
            string subject,
            string htmlBody)
        {
            var message = new MimeMessage();

            message.From.Add(
                new MailboxAddress(
                    _settings.DisplayName,
                    _settings.From));

            message.To.Add(
                MailboxAddress.Parse(email));

            message.Subject = subject;

            message.Body = new BodyBuilder
            {
                HtmlBody = htmlBody
            }.ToMessageBody();

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(
                _settings.Host,
                _settings.Port,
                _settings.UseSsl
                    ? SecureSocketOptions.StartTls
                    : SecureSocketOptions.Auto);

            await smtp.AuthenticateAsync(
                _settings.Username,
                _settings.Password);

            await smtp.SendAsync(message);

            await smtp.DisconnectAsync(true);
        }
    }
}