using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Text;
using Weather.Server.DTOs;

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

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            IOptions<OpenWeather> openWeather,
            HttpClient httpClient)
        {
            _httpClient = httpClient;
            _openWeather = openWeather.Value;
            _logger = logger;
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


                StringBuilder geocode = new StringBuilder();
                string geocodeUrl = geocode.Append(_openWeather.Site + _openWeather.GeoResponseType + _openWeather.GeoVersion)
                          .Append(_openWeather.GeolocationTemplate.Replace("cityname", cityName)
                          .Replace(",statecode", stateCode.HasValue ? stateCode.Value.ToString() : "")
                          .Replace(",countrycode", countryCode.HasValue ? countryCode.Value.ToString() : "")
                          .Replace("APIKey", _openWeather.Key)).ToString();

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
                var firstCity = geoCode.First();

                StringBuilder currentWeatherUrl = new StringBuilder();
                string currentUrl = currentWeatherUrl.Append(_openWeather.Site + _openWeather.WeatherResponseType + _openWeather.WeatherVersion)
                                .Append(_openWeather.CurrentWeatherTemplate.Replace("=lat", "=" + firstCity.Lat)
                                .Replace("=lon", "=" + firstCity.Lon).Replace("APIKey", _openWeather.Key)).ToString();

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
    }
}

