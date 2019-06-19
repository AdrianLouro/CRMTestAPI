using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entities.Contracts;

namespace Entities.Models
{
    [Table("user")]
    public class User : IEntity, IEntityModel
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("email")]
        [Required(ErrorMessage = "Email is required")]
        [StringLength(50, ErrorMessage = "Email can't be longer than 50 characters")]
        public string Email { get; set; }

        [Column("password")]
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "Password can't be longer than 100 characters")]
        public string Password { get; set; }

        [Column("name")]
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters")]
        public string Name { get; set; }

        [Column("surname")]
        [Required(ErrorMessage = "Surname is required")]
        [StringLength(100, ErrorMessage = "Surname can't be longer than 100 characters")]
        public string Surname { get; set; }

        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
    }
}
