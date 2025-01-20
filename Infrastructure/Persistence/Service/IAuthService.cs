
using Application.DTOs;


namespace Persistence.Service
{ 
	public interface IAuthService
	{
		Task<RegisterResponse> RegisterAsync(string email, string password);

		Task<LoginResponse> Login(string email, string password);
		Task<string> Confirmation(string email, int code);
	}
}

