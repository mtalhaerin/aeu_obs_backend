using Business.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IKullaniciService _kullaniciService;



        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            IKullaniciService kullaniciService)
        {
            _logger = logger;
            _kullaniciService = kullaniciService;
        }

        [HttpPost]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = "Warm",
            });
        }

        [HttpGet]


        public IActionResult Post()
        {
            return Ok(_kullaniciService.GetByUuid(
                Guid.Parse("11111111-1111-1111-1111-111111111111")
            ));
        }
    }
}
