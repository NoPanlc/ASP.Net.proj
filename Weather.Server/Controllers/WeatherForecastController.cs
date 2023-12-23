using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Text;
using Weather.Server.DTOs;
using Weather.Server.DTOs.CurrentWeather;
using Weather.Server.DTOs.FiveDaysWeather;
using Weather.Server.Interfaces;

namespace Weather.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly OpenWeather _openWeather;
        private readonly HttpClient _httpClient;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IUrlBuilderInterface _urlBuilder;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            IOptions<OpenWeather> openWeather,
            HttpClient httpClient,IUrlBuilderInterface urlBuilder)
        {
            _httpClient = httpClient;
            _openWeather = openWeather.Value;
            _logger = logger;
            _urlBuilder = urlBuilder;
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
    }
}

