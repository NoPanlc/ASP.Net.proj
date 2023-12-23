using Weather.Server.DTOs;

namespace Weather.Server.Interfaces
{
    public interface IUrlBuilderInterface
    {
        string GeoCodeUrl (OpenWeather openWeather, string cityName, int? stateCode, int? countryCode);

        string WeatherUrl (string template, OpenWeather openWeather, GeoCodeDTO Name);
    }
}
