namespace Weather.Server.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid TenantId { get; set; }
        public Tenant? Tenant { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; }
        public string Address { get; set; }

    }
}
