using System;
using Application.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Persistence.Service
{ 
	public interface IAuthService
	{
		Task<string> RegisterAsync(string email, string password);
		Task<IdentityUser?> GetUser(string email);
		Task<LoginResponse> Login(string email, string password);

	}
}

