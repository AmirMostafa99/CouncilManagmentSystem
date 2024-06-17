using System.ComponentModel.DataAnnotations;

namespace CouncilsManagmentSystem.DTOs
{
    public class UserForgetPasswordRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }
}
