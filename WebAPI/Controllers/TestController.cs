using Business.Concrete;
using Entities.Concrete.OzlukEntities;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly IKullaniciService _kullaniciService;

        public TestController(
            ILogger<TestController> logger,
            IKullaniciService kullaniciService)
        {
            _logger = logger;
            _kullaniciService = kullaniciService;
        }


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string userUuid)
        {
            var result = await _kullaniciService.GetByUuidAsync(Guid.Parse(userUuid));
            return Ok(result);
        }


    }
}
