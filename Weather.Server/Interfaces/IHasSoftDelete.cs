namespace Weather.Server.Interfaces
{
    public interface IHasSoftDelete
    {
        public DateTime? DeletedAt { get; set; }
    }
}
