using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using CouncilsManagmentSystem.Attributes;

namespace CouncilsManagmentSystem.DTOs
{
    public class AddUserDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? academic_degree { get; set; }
        public string? phone { get; set; }

        public string? functional_characteristic { get; set; }

        public string? administrative_degree { get; set; }

        [DefaultValue("defaultimage.png")]
        
        public string? img { get; set; }


        public DateTime? Birthday { get; set; }

        public int DepartmentId { get; set; }



    }
}
