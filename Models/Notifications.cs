using System.ComponentModel.DataAnnotations.Schema;

namespace CouncilsManagmentSystem.Models
{
    public class Notifications
    {
      
        public int Id { get; set; }

        public bool IsSeen { get; set; }

        public string MemberId { get; set; }
        public int CouncilId { get; set; }

        [ForeignKey("MemberId,CouncilId")]
        public CouncilMembers CouncilMembers { get; set; }
    }
}
