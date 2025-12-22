using Business.Concrete;
using Business.Features.CQRS.Auth.Login;
using Core.Entities.Concrete.OzlukEntities;
using Entities.Concrete.OzlukEntities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers.Base;

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
        [HttpPost("register")]
        public IActionResult Register()
        {
            return Ok();
        }

        // Email doğrulama
        [HttpGet("verify-email")]
        public IActionResult VerifyEmail()
        {
            return Ok();
        }

        // Parola kurtarma isteği
        [HttpPost("forgot-password")]
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
        public IActionResult ChangePassword()
        {
            return Ok();
        }

        // Giriş yapma
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            return await SendCommand(command);
        }

        // Çıkış yapma
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok();
        }

        // Token yenileme
        [HttpPost("refresh")]
        public IActionResult Refresh()
        {
            return Ok();
        }

        // Kullanıcı profili Görüntüleme
        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            return Ok();
        }

        // Kullanıcı profili Güncelleme
        [HttpPost("profile")]
        public IActionResult UpdateProfile([FromBody] Kullanici a, [FromQuery] int b, [FromHeader] int c)
        {
            return Ok();
        }
    }
}
