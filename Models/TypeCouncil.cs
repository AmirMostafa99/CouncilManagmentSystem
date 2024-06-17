using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CouncilsManagmentSystem.Models
{
    public class TypeCouncil
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("ChairmanCouncilId")]
        [Required(AllowEmptyStrings = true)]
        public string ChairmanCouncilId { get; set; }
        public ApplicationUser ChairmanCouncil { get; set; }

        [ForeignKey("SecretaryCouncilId")]
        [Required(AllowEmptyStrings = true)]
        public string SecretaryCouncilId { get; set; }
        public ApplicationUser SecretaryCouncil { get; set; }

        [ForeignKey("DepartmentId")]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public List<Councils> Councils { get; set; }
    }
}
