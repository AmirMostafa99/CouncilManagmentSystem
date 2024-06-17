using System.ComponentModel.DataAnnotations;

namespace CouncilsManagmentSystem.DTOs
{
    public class UserLoginRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
    }
}
