using Business.Concrete;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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


        public async Task<IActionResult> Post([FromQuery] string userUuid)
        {
            var result = await _kullaniciService.GetByUuidAsync(Guid.Parse(userUuid));
            return Ok(result);
        }
    }

    public class WeatherForecast
    {
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }
}