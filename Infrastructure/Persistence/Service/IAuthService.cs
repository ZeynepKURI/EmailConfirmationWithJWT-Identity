using System;
using Microsoft.AspNetCore.Identity;

namespace Persistence.Service
{ 
	public interface IAuthService
	{
		Task<string> RegisterAsync(string email, string password);
		Task<IdentityUser?> GetUser(string email);

	}
}

