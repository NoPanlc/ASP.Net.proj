using System.Runtime.Serialization;


namespace Weather.Server.DTOs.CurrentWeather
{
    public class CurrentWeatherDTO
    {
        public Main? Main { get; set; }
        public Wind? Wind { get; set; }
        public Clouds? Clouds { get; set; }
        public double Dt { get; set; }
    }
    public class Main
    {
        public double Temp { get; set; }
        public double Pressure { get; set; }
        public double Humidity { get; set; }
    }
    public class Wind
    {
        public double Speed { get; set; }
    }
    public class Clouds
    {
        public double All { get; set; }
    }
}
