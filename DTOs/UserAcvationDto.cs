using System.ComponentModel.DataAnnotations;

namespace CouncilsManagmentSystem.DTOs
{
    public class UserAcvationDto
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Formate")]
        public string Email { get; set; }

        [Required]
        public int Otp { get; set; }

        public bool IsVerified { get; set; }

        [MinLength(8, ErrorMessage = "Password Must be at least 8 characters"), DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
