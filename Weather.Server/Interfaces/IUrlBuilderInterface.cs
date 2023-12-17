using Weather.Server.DTOs;

namespace Weather.Server.Interfaces
{
    public interface IUrlBuilderInterface
    {
        string GeoCodeUrl (OpenWeather openWeather, string cityName, int? stateCode, int? countryCode);

        string WeatherUrl (OpenWeather openWeather, GeoCodeDTO Name);

        string FiveDaysUrl(OpenWeather openWeather, GeoCodeDTO Name);
    }
}
