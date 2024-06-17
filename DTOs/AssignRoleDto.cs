using System.ComponentModel.DataAnnotations;

namespace CouncilsManagmentSystem.DTOs
{
    public class AssignRoleDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string RoleName { get; set; }

        [Required]
        public bool IsSelected { get; set; }
    }
}
