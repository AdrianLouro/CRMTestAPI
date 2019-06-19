using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Entities.Contracts;

namespace Entities.Models.Reduced
{
    public class ReducedUser : IEntity, IEntityModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(50, ErrorMessage = "Email can't be longer than 50 characters")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Surname is required")]
        [StringLength(100, ErrorMessage = "Surname can't be longer than 100 characters")]
        public string Surname { get; set; }

        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
    }
}
