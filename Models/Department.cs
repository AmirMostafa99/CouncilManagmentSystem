using System.ComponentModel.DataAnnotations.Schema;

namespace CouncilsManagmentSystem.Models
{
    public class Department
    {
        public int id { get; set; }
        public string name { get; set; }

        [ForeignKey("Collage")]
        public int collage_id { get; set; }
        public Collage Collage { get; set; }

        // List<ApplicationUser> applicationUsers { get; set; }
        // public TypeCouncil AddCouncil { get; set; }
    }
}
