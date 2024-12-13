using ChaatyApi.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ChaatyApi.DTOs
{
    public class RigesterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Country { get; set; }
        
        public string City { get; set; }=string.Empty;
        public string LookingFor { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
    }
}
