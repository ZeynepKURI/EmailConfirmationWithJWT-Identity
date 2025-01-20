
using Application.DTOs;


namespace Persistence.Service
{ 
	public interface IAuthService
	{
		Task<string> RegisterAsync(string email, string password);

		Task<LoginResponse> Login(string email, string password);

	}
}

