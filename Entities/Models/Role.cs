using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Entities.Models
{
    [Table("role")]
    public class Role : IEntity
    {
        [Key] [Column("id")] public Guid Id { get; set; }

        [Column("type")]
        [Required(ErrorMessage = "Type is required")]
        [StringLength(50, ErrorMessage = "Type can't be longer than 50 characters")]
        public string Type { get; set; }

        [JsonIgnore]
        [Column("user")]
        [Required(ErrorMessage = "User ID is required")]
        public Guid UserId { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }
    }
}