using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models.Reduced
{
    public class UserProfile
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Surname is required")]
        [StringLength(100, ErrorMessage = "Surname can't be longer than 100 characters")]
        public string Surname { get; set; }
    }
}