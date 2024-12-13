using Microsoft.AspNetCore.Identity;

namespace ChaatyApi.Entities
{
    public class AppUser:IdentityUser
    {
        public AppUser()
        {
            Photos = new List<Photo>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string LookingFor { get; set; }
        public string Bio { get; set; }
        public DateTime CreatedAt { get; set; }=DateTime.Now;
        public DateTime LastActive { get; set; }=DateTime.Now;
        public ICollection<Photo> Photos { get; set; }
        public ICollection<Languages> Languages { get; set; }
        public ICollection<FriendShip> FriendshipsInitiated { get; set; }
        public ICollection<FriendShip> FriendshipsReceived { get; set; }
        public ICollection<Message> MessagesSent { get; set; }
        public ICollection<Message> MessagesReceived { get; set; }
    }
}
