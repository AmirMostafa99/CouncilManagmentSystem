using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CouncilsManagmentSystem.Models
{
    public class ApplicationUser : IdentityUser
    {

        public string FullName { get; set; }
        public int? OTP { get; set; }
        public bool? IsVerified { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Birthday { get; set; }

        public string? functional_characteristic { get; set; }
        public string? academic_degree { get; set; }
        public string? administrative_degree { get; set; }
        public string? Discription { get; set; }
        [DefaultValue("defaultimage.png")]
        public string? img { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        [InverseProperty("ChairmanCouncil")]
        public TypeCouncil Chairmanship { get; set; }

        [InverseProperty("SecretaryCouncil")]
        public TypeCouncil Secretaryship { get; set; }
        public ICollection<CouncilMembers> CouncilMembers { get; set; }
        public Permissionss Permissionss { get; set; }

    }
}
