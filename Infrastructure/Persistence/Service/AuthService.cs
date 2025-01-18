using System;
using System.Text;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Persistence.Service
{
	public class AuthService : IAuthService
	{

		private readonly UserManager<IdentityUser> _userManager;
		private readonly IConfiguration _configuration;


		public AuthService(UserManager<IdentityUser> userManager, IConfiguration configuration)
		{
			_userManager = userManager;
			_configuration = configuration;

		}


		public async Task<string> RegisterAsync(string email, string password)
		{
			var user = await GetUser(email);
			if (user != null)
				return "User already exists";

			var result = await _userManager.CreateAsync(new IdentityUser()
			{
				UserName = email,
				Email = email,
				PasswordHash = password
			}, password
			);


			if (!result.Succeeded)

				return "User registered successfully!";

			var _user = await GetUser(email);
			var emailCode = await _userManager.GenerateEmailConfirmationTokenAsync(_user);
            await SendEmail(email, emailCode);
            return "EMAİL SEND SUCCESSFULLY";
		}

        private async Task SendEmail(string email, string emailCode)
        {
            // HTML mesajını oluşturuyoruz
            StringBuilder emailMessage = new StringBuilder();
            emailMessage.AppendLine("<html>");
            emailMessage.AppendLine("<body>");
            emailMessage.AppendLine($"<p>Dear {email},</p>");
            emailMessage.AppendLine("<p>Thank you for registering with us. To verify your email address, please use the code below:</p>");
            emailMessage.AppendLine($"<h2>Verification Code: {emailCode}</h2>");
            emailMessage.AppendLine("<p>Please enter this code on our website to complete your registration process.</p>");
            emailMessage.AppendLine("<p>If you did not request this, please ignore this email.</p>");
            emailMessage.AppendLine("<br>");
            emailMessage.AppendLine("<p>Best regards, </p>");
            emailMessage.AppendLine("<p><strong>Netcode-Hub</strong></p>");
            emailMessage.AppendLine("</body>");
            emailMessage.AppendLine("</html>");

			string message = emailMessage.ToString();
            var _email= new MimeMessage();
			_email.To.Add(MailboxAddress.Parse(""));
          _email.From.Add(MailboxAddress.Parse(""));
           _email.Subject = "Email Verification";
			_email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message };


            // Send the email via SMTP
            using var smtp = new SmtpClient();
       
            smtp.Authenticate("",""); // Authenticate
           smtp.Send(_email); // Send email
           smtp.DisconnectAsync(true);
        }
        public async Task<IdentityUser?> GetUser(string email)
		{

			return await _userManager.FindByEmailAsync(email);


        }
    }
}

