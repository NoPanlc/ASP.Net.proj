namespace Weather.Server.Interfaces
{
    public interface IHasDateStamp
    {
        public DateTime CreatedAt { get; set; }
    }
}
