namespace ChaatyApi.Entities
{
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; } = false;
        public string PublicId { get; set; } = string.Empty;
        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
    }
}
