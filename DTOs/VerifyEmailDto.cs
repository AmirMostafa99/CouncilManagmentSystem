using System.ComponentModel.DataAnnotations;

namespace CouncilsManagmentSystem.DTOs
{
    public class VerifyEmailDto
    {
        [Required]
        public string Email { get; set; }


    }
}
