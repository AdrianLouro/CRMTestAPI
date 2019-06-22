using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entities.Contracts;
using Newtonsoft.Json;

namespace Entities.Models
{
    [Table("customer")]
    public class Customer : IEntity, IEntityModel
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("name")]
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters")]
        public string Name { get; set; }

        [Column("surname")]
        [Required(ErrorMessage = "Surname is required")]
        [StringLength(100, ErrorMessage = "Surname can't be longer than 100 characters")]
        public string Surname { get; set; }

        [JsonIgnore]
        [Column("photo_name")]
        [StringLength(50, ErrorMessage = "Photo name can't be longer than 50 characters")]
        public string PhotoName { get; set; }

        [JsonIgnore]
        [Column("created_by")]
        public Guid CreatedById { get; set; }

        [JsonIgnore]
        public virtual User CreatedBy { get; set; }

        [JsonIgnore]
        [Column("last_updated_by")]
        public Guid? LastUpdatedById { get; set; }

        [JsonIgnore]
        public virtual User LastUpdatedBy { get; set; }

        [NotMapped]
        public String PhotoPath { get; set; }
    }
}
