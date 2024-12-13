using ChaatyApi.Entities;

namespace ChaatyApi.DTOs
{
    public class UserDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhotoUrl { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Country { get; set; }
        
        public ICollection<PhotoToRreturnDto> Photos { get; set; }
        public ICollection<Languages> Languages { get; set; }
    }
}
