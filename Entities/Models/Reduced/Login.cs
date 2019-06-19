using System.ComponentModel.DataAnnotations;
using Entities.Contracts;

namespace Entities.Models.Reduced
{
    public class Login: IEntityModel
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}