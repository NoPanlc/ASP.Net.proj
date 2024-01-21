namespace Weather.Server.Models
{
    public class Tenant
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<User>? Users { get; set; }
        public ICollection<Record>? Records { get; set; }
        public ICollection<CurrentWeather>? CurrentWeatherCalls { get; set; }
        public ICollection<FiveDaysWeather>? FiveDaysWeatherCalls { get; set; }

    }
}
