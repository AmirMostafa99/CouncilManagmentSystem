using System.ComponentModel.DataAnnotations;

namespace CouncilsManagmentSystem.DTOs
{
    public class ChangePasswordDto
    {
        [Required]
        public string OldPassword { get; set; }
        [Required]

        public string NewPassword { get; set; }
        [Required]

        public string ConfirmNewPassword { get; set; }
      

    }
}
