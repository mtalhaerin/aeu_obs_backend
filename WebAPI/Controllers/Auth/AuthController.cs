using Business.Concrete;
using Business.Features.CQRS.Auth.Login;
using Business.Features.CQRS.Auth.Logout;
using Business.Features.CQRS.Auth.Refresh;
using Business.Features.CQRS.Auth.Validate;
using Business.Features.CQRS.Dashboard.Profile.Query;
using Core.Entities.Concrete.OzlukEntities;
using Entities.Concrete.OzlukEntities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.Controllers.Base;
using WebAPI.Filters;

namespace WebAPI.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : GenericController
    {
        public AuthController(
            IMediator mediator,
            ILogger<AuthController> logger) : base(mediator, logger)
        {
        }

        // Kayıt olma
        //[HttpPost("register")]
        //[RejectAuthorizationHeader]
        //public IActionResult Register()
        //{
        //    return Ok();
        //}

        // Email doğrulama
        //[HttpGet("verify-email")]
        //[RejectAuthorizationHeader]
        //public IActionResult VerifyEmail()
        //{
        //    return Ok();
        //}

        // Parola kurtarma isteği
        //[HttpPost("forgot-password")]
        //[RejectAuthorizationHeader]
        //public IActionResult ForgotPassword()
        //{
        //    return Ok();
        //}

        // Şifre sıfırlama isteği
        //[HttpPost("reset-password")]
        //public IActionResult ResetPassword()
        //{
        //    return Ok();
        //}

        // Şifre değiştirme
        //[HttpPost("change-password")]
        //[Authorize]
        //public IActionResult ChangePassword()
        //{
        //    return Ok();
        //}

        // Giriş yapma
        [HttpPost("login")]
        [RejectAuthorizationHeader]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            return await SendCommand(command);
        }

        // Çıkış yapma
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromHeader(Name = "Authorization")] LogoutCommand command)
        {
            return await SendCommand(command);
        }

        // Token yenileme
        [HttpPost("refresh")]
        [Authorize]
        public async Task<IActionResult> Refresh([FromHeader(Name = "Authorization")] RefreshCommand command)
        {
            return await SendCommand(command);
        }

        // Token validating
        [HttpPost("validate")]
        [Authorize]
        public async Task<IActionResult> Validate([FromHeader(Name = "Authorization")] ValidateCommand command)
        {
            return await SendCommand(command);
        }
    }
}
