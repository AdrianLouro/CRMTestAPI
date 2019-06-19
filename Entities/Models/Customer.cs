using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entities.Contracts;

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

        [Column("photo_extension")]
        [StringLength(10, ErrorMessage = "Photo extension can't be longer than 10 characters")]
        public string PhotoExtension { get; set; }

        [Column("created_by")]
        public Guid CreatedById { get; set; }

        public virtual User CreatedBy { get; set; }

        [Column("last_updated_by")]
        public Guid LastUpdatedById { get; set; }

        public virtual User LastUpdatedBy { get; set; }
    }
}
