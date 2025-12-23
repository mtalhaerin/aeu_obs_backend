using Business.Concrete;
using Business.Features.CQRS.Auth.Login;
using Business.Features.CQRS.Auth.Logout;
using Core.Entities.Concrete.OzlukEntities;
using Entities.Concrete.OzlukEntities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers.Base;
using WebAPI.Filters;

namespace WebAPI.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : GenericController
    {
        private readonly IKullaniciService _kullaniciService;

        public AuthController(
            IMediator mediator,
            ILogger<AuthController> logger,
            IKullaniciService kullaniciService) : base(mediator, logger)
        {
            _kullaniciService = kullaniciService;
        }

        // Kayıt olma
        //[HttpPost("register")]
        //[RejectAuthorizationHeader]
        //public IActionResult Register()
        //{
        //    return Ok();
        //}

        // Email doğrulama
        [HttpGet("verify-email")]
        [RejectAuthorizationHeader]
        public IActionResult VerifyEmail()
        {
            return Ok();
        }

        // Parola kurtarma isteği
        [HttpPost("forgot-password")]
        [RejectAuthorizationHeader]
        public IActionResult ForgotPassword()
        {
            return Ok();
        }

        // Şifre sıfırlama isteği
        [HttpPost("reset-password")]
        public IActionResult ResetPassword()
        {
            return Ok();
        }

        // Şifre değiştirme
        [HttpPost("change-password")]
        [Authorize]
        public IActionResult ChangePassword()
        {
            return Ok();
        }

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
        public IActionResult Logout([FromHeader(Name = "Authorization")] LogoutCommand command)
        {
            return Ok();
        }

        // Token yenileme
        [HttpPost("refresh")]
        [Authorize]
        public IActionResult Refresh()
        {
            return Ok();
        }

        // Kullanıcı profili Görüntüleme
        [HttpGet("profile")]
        [Authorize]
        public IActionResult GetProfile()
        {
            return Ok();
        }

        // Kullanıcı profili Güncelleme
        [HttpPost("profile")]
        [Authorize]
        public IActionResult UpdateProfile([FromBody] Kullanici a, [FromQuery] int b, [FromHeader] int c)
        {
            return Ok();
        }
    }
}
