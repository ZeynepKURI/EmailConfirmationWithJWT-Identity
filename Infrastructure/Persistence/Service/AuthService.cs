using System;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.DTOs;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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


		public async Task<RegisterResponse> RegisterAsync(string email, string password)
		{
			var user = await GetUser(email);
			if (user != null)
				return new RegisterResponse(false,"User already exists");

			var result = await _userManager.CreateAsync(new IdentityUser()
			{
				UserName = email,
				Email = email,
				PasswordHash = password
			}, password
			);


			if (!result.Succeeded)

				return new RegisterResponse( true, "User registered successfully!");

			var _user = await GetUser(email);
			var emailCode = await _userManager.GenerateEmailConfirmationTokenAsync(_user);
            await SendEmail(email, emailCode);
            return new RegisterResponse(true,"EMAİL SEND SUCCESSFULLY");
		}

        private async Task<string> SendEmail(string email, string emailCode)
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
            emailMessage.AppendLine("<p>Best regards,</p>");
            emailMessage.AppendLine("<p><strong>Netcode-Hub</strong></p>");
            emailMessage.AppendLine("</body>");
            emailMessage.AppendLine("</html>");



            string message = emailMessage.ToString();

            var _email= new MimeMessage();
			_email.To.Add(MailboxAddress.Parse("mathilde.muller@ethereal.email"));
          _email.From.Add(MailboxAddress.Parse("mathilde.muller@ethereal.email"));
           _email.Subject = "Email Verification";
			_email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message };


            // Send the email via SMTP
            using var smtp = new SmtpClient();
                // SMTP sunucusunu ve portunu belirtiyoruz
            smtp.Connect("smtp.ethereal.email",587, MailKit.Security.SecureSocketOptions.StartTls);

            // E-posta adresinizi ve şifrenizi girerek kimlik doğrulaması yapıyoruz
            smtp.Authenticate("mathilde.muller@ethereal.email", "9Kpre5n6gNdqHe7FFB");
            // E-postayı gönderiyoruz
            smtp.Send(_email);

            // Bağlantıyı düzgün bir şekilde sonlandırıyoruz
            smtp.Disconnect(true);

            return "Thank you for your registration. Kindly check your email for the confirmation code.";

        }

        // Email doğrulama işlemi
        public async Task<string> Confirmation(string email , int code)
        {
            if (string.IsNullOrEmpty(email) || code <= 0)
                return "Invalid code provided";

            var User = await GetUser(email);
            if (User == null)
                return "Invalid identity provided";

            var result = await _userManager.ConfirmEmailAsync(User, code.ToString());
            if (!result.Succeeded)
                return "Invalid code provided";
            else
                return "Email cofirmed successfully, yout can proceed to login";
        }

        // Kullanıcı girişi işlemi
        public async Task<LoginResponse> Login(string email , string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return new LoginResponse ( false, "Invalid email or password." );


            var user = await GetUser(email);


            bool isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            if (!isEmailConfirmed)
                return new LoginResponse(false, "You need to confirm your email before logging in.");

            string token = GenerateToken(user);
            return new LoginResponse(true, token);



        }

        // Token üretme

        private string GenerateToken(IdentityUser? user)
        {
            byte [] key = Encoding.ASCII.GetBytes("Qw12ER34TY56Ui78oi98v2bNh78JK4Hods7uUj12");
            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user!.Id),
                new Claim(JwtRegisteredClaimNames.Email, user!.Email!)

            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(5),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

       private async Task<IdentityUser?> GetUser(string email)
		{

			return await _userManager.FindByEmailAsync(email);


        }
    }
}

