using System.ComponentModel.DataAnnotations;

namespace CouncilsManagmentSystem.DTOs
{
    public class ResetPasswordRequestDto
    {
        [DataType(DataType.Password)]
        [Required]
        public string NewPassword { get; set; }

        [Required]
        public string Email { get; set; }


        [Required]
        public int OTP { get; set; }

    }
}
