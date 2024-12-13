namespace ChaatyApi.DTOs
{
    public class UserDetailsDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhotoUrl { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string LookingFor { get; set; }
        public string Bio { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastActive { get; set; }
        public bool isThereFriendRequest { get; set; } = false;
        public bool IsSender { get; set; }  
        public string FriendshipStatus { get; set; }
        public int FriendShipId { get; set; }
        public ICollection<PhotoToRreturnDto> Photos { get; set; }
    }
}
