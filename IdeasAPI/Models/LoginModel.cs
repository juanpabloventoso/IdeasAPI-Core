using System.ComponentModel.DataAnnotations;

namespace IdeasAPI.Models
{
    public class LoginModel
    {

        [DataType(DataType.EmailAddress)]
        public string email { get; set; }

        [Required]
        [RegularExpression(@"(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).*",
            ErrorMessage = "Password must have at least 8 characters, including 1 uppercase letter, 1 lowercase letter, and 1 number")]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}
