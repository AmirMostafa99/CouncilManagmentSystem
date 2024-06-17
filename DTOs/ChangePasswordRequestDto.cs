using System.ComponentModel.DataAnnotations;

namespace CouncilsManagmentSystem.DTOs
{
    public class ChangePasswordRequestDto
    {
        
        [Required]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string NewPassword { get; set; }
    }
}
