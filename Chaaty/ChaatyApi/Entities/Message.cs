namespace ChaatyApi.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string SenderFirstName { get; set; }
        public AppUser Sender { get; set; }

        public string RecipientId { get; set; }
        public string RecipientFirstName { get; set; }
        public AppUser Recipient { get; set; }

        public string Content { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; } = DateTime.UtcNow;

        public bool SenderDeleted { get; set; }
        public bool RecipientDeleted { get; set; }
    }
}
