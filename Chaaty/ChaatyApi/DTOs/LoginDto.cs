﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ChaatyApi.DTOs
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }
    }
}
