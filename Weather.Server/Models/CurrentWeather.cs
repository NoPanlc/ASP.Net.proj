﻿using Weather.Server.Interfaces;

namespace Weather.Server.Models
{
    public class CurrentWeather : IHasGuidId, IHasDateStamp, IHasSoftDelete
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
