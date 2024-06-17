using System.ComponentModel.DataAnnotations;

namespace CouncilsManagmentSystem.DTOs
{
    public class AddTypeCouncilsDTO
    {

        public string Name { get; set; }
        [Required(AllowEmptyStrings = true)]
        public string ChairmanCouncilEmail { get; set; }
        [Required(AllowEmptyStrings = true)]
        public string SecretaryCouncilEmail { get; set; }

        public int DepartmentId { get; set; }



    }
}
