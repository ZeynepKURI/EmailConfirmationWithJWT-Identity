using System;
namespace Application.DTOs
{
	public record RegisterResponse ( bool Flag , string Message ,string Token = null);

}

