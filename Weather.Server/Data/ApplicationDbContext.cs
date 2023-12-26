using Microsoft.EntityFrameworkCore;
using Weather.Server.Models;

namespace Weather.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Record> Records { get; set; }

        public DbSet<CurrentWeather> CurrentWeatherCalls { get; set; }

        public DbSet<FiveDaysWeather> FiveDaysWeatherCalls { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
