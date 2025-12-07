using Business.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IKullaniciService _kullaniciService;

        public AuthController(
            ILogger<AuthController> logger,
            IKullaniciService kullaniciService)
        {
            _logger = logger;
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
        public IActionResult Login()
        {
            return Ok();
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
        public IActionResult UpdateProfile()
        {
            return Ok();
        }
    }
}
