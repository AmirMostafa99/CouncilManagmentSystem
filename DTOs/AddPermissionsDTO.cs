using CouncilsManagmentSystem.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace CouncilsManagmentSystem.DTOs
{
    public class AddUsersWithPermissionsDTO
    {
        public List<AddPermissionsDTO> UsersPermissions { get; set; }
    }
    public class AddPermissionsDTO
    {
        public string UserId { get; set; }


        [DefaultValue(false)]
        public bool AddCouncil { get; set; }
        [DefaultValue(false)]
        public bool EditCouncil { get; set; }
        [DefaultValue(false)]
        public bool CreateTypeCouncil { get; set; }
        [DefaultValue(false)]
        public bool EditTypeCouncil { get; set; }
        [DefaultValue(false)]
        public bool AddMembersByExcil { get; set; }
        [DefaultValue(false)]
        public bool AddMembers { get; set; }
        [DefaultValue(false)]
        public bool AddTopic { get; set; }
        [DefaultValue(false)]
        public bool Arrange { get; set; }
        [DefaultValue(false)]
        public bool AddResult { get; set; }

      
   

    }
}
