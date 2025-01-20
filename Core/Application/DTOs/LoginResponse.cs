using System;
namespace Application.DTOs
{
    public record LoginResponse(bool Flag, string Message, string Token = null);


}

