using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Persistence.Service;

namespace Api.Controllers
{
    public class AccountController : ControllerBase
    {
        private readonly AuthService _authService;
     

        public AccountController ( AuthService authService, IConfiguration configuration)
        {
          
            _authService = authService;
        }


        [HttpPost ("login/{email}/{password}")]
        public async Task<IActionResult> LoginAsync(string email, string password)
        {
           
            try
            {
                var user = await _authService.Login(email, password);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("register/{email}/{passeord}")]
        public async Task <IActionResult> Register(string email, string password)
        {
            try
            {
                var user = await _authService.RegisterAsync(email, password);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}