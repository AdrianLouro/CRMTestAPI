using System;
using System.ComponentModel.DataAnnotations;
using Entities;

namespace Entities.Models
{
    public class LoginModel : IEntity
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}