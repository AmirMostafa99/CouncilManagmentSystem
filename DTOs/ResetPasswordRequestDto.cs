using System.ComponentModel.DataAnnotations;

namespace CouncilsManagmentSystem.DTOs
{
    public class ConfirmOTPDto
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public int OTP { get; set; }

    }
}
