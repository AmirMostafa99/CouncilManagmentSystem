using System.ComponentModel.DataAnnotations.Schema;

namespace CouncilsManagmentSystem.Models
{
    public class Councils
    {
        public int Id { get; set; } 
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public int HallId { get; set; }
        public Hall Hall { get; set; }
        [ForeignKey("TypeCouncil")]
        public int TypeCouncilId { get; set; }
        public TypeCouncil TypeCouncil { get; set; }
        public ICollection<Topic> TopicS { get; set; }
        public ICollection<CouncilMembers> CouncilMembers { get; set; }

    }
}
