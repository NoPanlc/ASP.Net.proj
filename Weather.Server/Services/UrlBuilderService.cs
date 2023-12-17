using System.Text;
using Weather.Server.DTOs;
using Weather.Server.Interfaces;

namespace Weather.Server.Services
{
    public class UrlBuilderService: IUrlBuilderInterface
    {
        public string GeoCodeUrl(OpenWeather openWeather, string cityName, int? stateCode, int? countryCode) 
        {
            StringBuilder geocode = new StringBuilder();
            string geocodeUrl = geocode.Append(openWeather.Site + openWeather.GeoResponseType + openWeather.GeoVersion)
                      .Append(openWeather.GeolocationTemplate.Replace("cityname", cityName)
                      .Replace(",statecode", stateCode.HasValue ? stateCode.Value.ToString() : "")
                      .Replace(",countrycode", countryCode.HasValue ? countryCode.Value.ToString() : "")
                      .Replace("APIKey", openWeather.Key)).ToString();
            return geocodeUrl;
        }
        public string WeatherUrl(OpenWeather openWeather, GeoCodeDTO Name)
        {
            StringBuilder currentWeatherUrl = new StringBuilder();
            string currentUrl = currentWeatherUrl.Append(openWeather.Site + openWeather.WeatherResponseType + openWeather.WeatherVersion)
                                .Append(openWeather.CurrentWeatherTemplate.Replace("=lat", "=" + Name.Lat)
                                .Replace("=lon", "=" + Name.Lon).Replace("APIKey", openWeather.Key)).ToString();
            return currentUrl;
        }
        public string FiveDaysUrl(OpenWeather openWeather, GeoCodeDTO Name) 
        {
            StringBuilder fiveDaysUrl = new StringBuilder();
            string currentUrl = fiveDaysUrl.Append(openWeather.Site + openWeather.WeatherResponseType + openWeather.WeatherVersion)
                                .Append(openWeather.FiveDaysForecastTemplate.Replace("=lat", "=" + Name.Lat)
                                .Replace("=lon", "=" + Name.Lon).Replace("APIKey", openWeather.Key)).ToString();

            return currentUrl;
        }
    }
}
