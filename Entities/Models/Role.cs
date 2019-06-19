using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entities.Contracts;
using Newtonsoft.Json;

namespace Entities.Models
{
    [Table("role")]
    public class Role : IEntity, IEntityModel
    {
        [Key] [Column("id")] public Guid Id { get; set; }

        [Column("type")]
        [Required(ErrorMessage = "Type is required")]
        [StringLength(50, ErrorMessage = "Type can't be longer than 50 characters")]
        public string Type { get; set; }

        [Column("user")]
        [Required(ErrorMessage = "User ID is required")]
        public Guid UserId { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }
    }
}