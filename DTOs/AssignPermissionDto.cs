using System.ComponentModel.DataAnnotations;

namespace CouncilsManagmentSystem.DTOs
{
    public class AssignPermissionDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Permission { get; set; }

        [Required]
        public bool IsSelected { get; set; }
    }
}
