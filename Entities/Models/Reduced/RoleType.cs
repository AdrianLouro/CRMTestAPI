using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entities.Contracts;

namespace Entities.Models.Reduced
{
    [Table("role")]
    public class RoleType : IEntityModel
    {
        [Required(ErrorMessage = "Type is required")]
        [StringLength(50, ErrorMessage = "Type can't be longer than 50 characters")]
        public string Type { get; set; }
    }
}