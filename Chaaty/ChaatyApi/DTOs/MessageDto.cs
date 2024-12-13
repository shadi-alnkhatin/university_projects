using ChaatyApi.Entities;

namespace ChaatyApi.DTOs
{
    public class MessageDto
    {
      
            public int Id { get; set; }
            public string SenderId { get; set; }
            public string SenderFirstName { get; set; }
            public string SenderPhotoUrl { get; set; }

            public string RecipientId { get; set; }
            public string RecipientFirstName { get; set; }
            public string RecipientPhotoUrl { get; set; }

            public string Content { get; set; }
            public DateTime? DateRead { get; set; }
            public DateTime MessageSent { get; set; } 
           
     
 }
}
