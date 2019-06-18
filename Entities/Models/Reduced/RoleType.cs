using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models.Reduced
{
    [Table("role")]
    public class RoleType
    {
        [Required(ErrorMessage = "Type is required")]
        [StringLength(50, ErrorMessage = "Type can't be longer than 50 characters")]
        public string Type { get; set; }
    }
}