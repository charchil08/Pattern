
using System.ComponentModel.DataAnnotations;

namespace Pattern.Models
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Invalid password")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Invalid Password")]
        public string Password { get; set; } = null!;
    }
}
