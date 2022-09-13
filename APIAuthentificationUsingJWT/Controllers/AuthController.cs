using APIAuthentificationUsingJWT.DTOS;
using APIAuthentificationUsingJWT.Helpers;
using APIAuthentificationUsingJWT.Models;
using APIAuthentificationUsingJWT.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace APIAuthentificationUsingJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly JwtService _jwtService;

        public AuthController(IUserRepository repository, JwtService jwtService)
        {
            _repository = repository;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDTO dto)
        {
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };
                     
            return Created("success", _repository.Create(user));
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDTO dto)
        {
            var user = _repository.GetByEmail(dto.Email);

            if (user == null) return BadRequest(new {message = "Invalid Credentials"});

            if(!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password)) return BadRequest(new { message = "Invalid Credentials" });

            var jwt = _jwtService.Generate(user.Id);

            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                SameSite = SameSiteMode.None,
                Secure = true,
                HttpOnly = true,
            });
            
            return Ok(new 
            { 
                message = "Success" 
            });
        }

        [HttpGet("user")]
        public IActionResult User()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];

                var token = _jwtService.Verify(jwt);

                int userId = int.Parse(token.Issuer);

                var user = _repository.GetById(userId);
            
                return Ok(user);
            }
            catch (Exception)
            {
                return Unauthorized();
            }
            
        }

        [HttpPost("logout")]
        public IActionResult LogOut()
        {
            Response.Cookies.Delete("jwt", new CookieOptions()
            {
                SameSite = SameSiteMode.None,
                Secure = true
            });

           
            return Ok(new {message = "success"});
        }
    }
}
