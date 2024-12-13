using Microsoft.AspNet.Identity;

namespace ChaatyApi.Entities
{
    public class FriendShip
    {
        public int Id { get; set; }
        public string SenderUserId { get; set; }
        public string ReceiverUserId { get; set; }
        public string FriendshipStatus { get; set; } // Pending, Accepted, Rejected
        public DateTime RequestedAt { get; set; }=DateTime.Now;
        public AppUser Sender{ get; set; }
        public AppUser Receiver { get; set; }
    }
}
