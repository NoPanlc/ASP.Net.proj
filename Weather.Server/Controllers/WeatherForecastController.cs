using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Text;
using Weather.Server.DTOs;
using Weather.Server.DTOs.CurrentWeather;
using Weather.Server.DTOs.FiveDaysWeather;
using Weather.Server.Interfaces;
using Weather.Server.Data;
using Microsoft.EntityFrameworkCore;
using Weather.Server.Models;

namespace Weather.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly OpenWeather _openWeather;
        private readonly HttpClient _httpClient;
        private ApplicationDbContext _context;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IUrlBuilderInterface _urlBuilder;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            IOptions<OpenWeather> openWeather,
            HttpClient httpClient,IUrlBuilderInterface urlBuilder,
            ApplicationDbContext context)
        {
            _httpClient = httpClient;
            _openWeather = openWeather.Value;
            _logger = logger;
            _urlBuilder = urlBuilder;
            _context = context;
        }


        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
       
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpGet("CurrentWeather")]
        public async Task<ActionResult<CurrentWeatherDTO>> GetCurrentWeather([FromQuery][Required] string cityName,
                                                   [FromQuery] int? stateCode,
                                                  [FromQuery] int? countryCode)
        {
            try
            {
                if (_openWeather == null || string.IsNullOrWhiteSpace(cityName))
                {
                    return BadRequest("Some configuration or request is empty");
                }


                string geocodeUrl = _urlBuilder.GeoCodeUrl(_openWeather, cityName, stateCode, countryCode);

                var geoResponse = await _httpClient.GetAsync(geocodeUrl);

                if (!geoResponse.IsSuccessStatusCode || geoResponse == null || geoResponse.Content == null)
                {
                    return BadRequest("Call to Open Weather for geocode failed");
                }

                string geo = await geoResponse.Content.ReadAsStringAsync();
                var geoCode = JsonConvert.DeserializeObject<List<GeoCodeDTO>>(geo);

                if (geoCode == null || geoCode == null || geoCode.Count == 0)
                {
                    return BadRequest("Deserialization of geocode failed");
                }
                var Name = geoCode.First();

                string currentUrl = _urlBuilder.WeatherUrl(_openWeather.CurrentWeatherTemplate, _openWeather, Name);

                var currentWeatherResponse = await _httpClient.GetAsync(currentUrl);
                if (!currentWeatherResponse.IsSuccessStatusCode || currentWeatherResponse == null || currentWeatherResponse.Content == null)
                {
                    return BadRequest("Call to Open Weather for current weather failed");
                }

                string current = await currentWeatherResponse.Content.ReadAsStringAsync();
                var currentWeather = JsonConvert.DeserializeObject<CurrentWeatherDTO>(current);
                if (currentWeather == null)
                {
                    return BadRequest("Deserialization of current weather failed");
                }

                return Ok(currentWeather);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }



        }
        [HttpGet("FiveDaysWeather")]
        public async Task<ActionResult<FiveDaysWeatherDTO>> GetFiveDaysWeather([FromQuery][Required] string cityName,
                                                   [FromQuery] int? stateCode,
                                                  [FromQuery] int? countryCode)
        {
            try
            {
                if (_openWeather == null || string.IsNullOrWhiteSpace(cityName))
                {
                    return BadRequest("Some configuration or request is empty");
                }


                string geocodeUrl = _urlBuilder.GeoCodeUrl(_openWeather, cityName, stateCode, countryCode);

                var geoResponse = await _httpClient.GetAsync(geocodeUrl);

                if (!geoResponse.IsSuccessStatusCode || geoResponse == null || geoResponse.Content == null)
                {
                    return BadRequest("Call to Open Weather for geocode failed");
                }

                string geo = await geoResponse.Content.ReadAsStringAsync();
                var geoCode = JsonConvert.DeserializeObject<List<GeoCodeDTO>>(geo);

                if (geoCode == null || geoCode == null || geoCode.Count == 0)
                {
                    return BadRequest("Deserialization of geocode failed");
                }
                var Name = geoCode.First();

                string currentUrl = _urlBuilder.WeatherUrl(_openWeather.FiveDaysForecastTemplate,_openWeather, Name);

                var fiveDaysWeatherResponse = await _httpClient.GetAsync(currentUrl);
                if (!fiveDaysWeatherResponse.IsSuccessStatusCode || fiveDaysWeatherResponse == null || fiveDaysWeatherResponse.Content == null)
                {
                    return BadRequest("Call to Open Weather for current weather failed");
                }

                string current = await fiveDaysWeatherResponse.Content.ReadAsStringAsync();
                var fiveDaysWeather = JsonConvert.DeserializeObject<FiveDaysWeatherDTO>(current);
                if (fiveDaysWeather == null)
                {
                    return BadRequest("Deserialization of current weather failed");
                }

                return Ok(fiveDaysWeather);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }



        }
        [HttpPost("SeedDefaultTenantsAndUsers")]
        public async Task<ActionResult> SeedDefaultTenantsAndUsers()
        {
            int result = 0;
            List<Tenant> tenants = new List<Tenant>
            {
                new Tenant
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.Now,
                    Name = "Agroworld"
                },
                new Tenant
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.Now,
                    Name = "Translogistic"
                },
                new Tenant
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.Now,
                    Name = "HuntSeason"
                },
                new Tenant
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.Now,
                    Name = "Fisher"
                }
            };

            if (!_context.Tenants.Any())
            {
                await _context.AddRangeAsync(tenants);
                result += await _context.SaveChangesAsync();
                tenants = await _context.Tenants.ToListAsync();
            }

            List<User> users = new List<User>()
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.Now,
                    Name = "Albert",
                    Email = "albert@gmail.com",
                    Address = "Lviv",
                    TenantId = tenants.First(x => x.Name == "Agroworld").Id
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.Now,
                    Name = "Andy",
                    Email = "andy@gmail.com",
                    Address = "Damask",
                    TenantId = tenants.First(x => x.Name == "Translogistic").Id
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.Now,
                    Name = "Conrad",
                    Email = "conrad@gmail.com",
                    Address = "Donetsk",
                    TenantId = tenants.First(x => x.Name == "HuntSeason").Id
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.Now,
                    Name = "Donald",
                    Email = "donald@gmail.com",
                    Address = "Kyiv",
                    TenantId = tenants.First(x => x.Name == "Fisher").Id
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.Now,
                    Name = "John",
                    Email = "John@gmail.com",
                    Address = "Krakiw",
                    TenantId = tenants.First(x => x.Name == "Agroworld").Id
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.Now,
                    Name = "Dean",
                    Email = "dean@gmail.com",
                    Address = "Kherson",
                    TenantId = tenants.First(x => x.Name == "HuntSeason").Id
                }
            };

            if (!_context.Users.Any())
            {
                await _context.AddRangeAsync(users);
                result += await _context.SaveChangesAsync();
            }

            if (result == 0)
            {
                return BadRequest($"Seed method affected {result} rows in the database");
            }
            return Ok($"Seed method affected {result} rows in the database");
        }

    }
}
    


