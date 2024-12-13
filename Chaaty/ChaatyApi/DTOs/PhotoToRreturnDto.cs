namespace ChaatyApi.DTOs
{
    public class PhotoToRreturnDto
    {
         public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; } = false;
    }
}
