using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Persistence.Service;

namespace Persistence.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection InfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // PostgreSQL bağlantı dizesini appsettings.json'dan alıyoruz
            var connectionString = configuration.GetConnectionString("Default");

            // DbContext ile Npgsql bağlantısını ekliyoruz
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));

            // JWT authentication yapılandırması
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                // JWT anahtarını appsettings.json'dan alıyoruz
                var secretKey = configuration["Jwt:SecretKey"];
                var key = Encoding.ASCII.GetBytes(secretKey);

                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                };
            });

            // Identity yapılandırması
            services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
            })
            .AddEntityFrameworkStores<AppDbContext>();

            // AuthService'i DI konteynerine kaydediyoruz
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
