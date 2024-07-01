using System.ComponentModel.DataAnnotations.Schema;

namespace CouncilsManagmentSystem.Models
{
    public class Vote
    {
        public int Id { get; set; }
        public int TopicId { get; set; }
        public int Agree { get; set; }
        public int Disagree { get; set; }
        public int Abstaining { get; set; }

        [ForeignKey("TopicId")]
        public Topic Topic { get; set; }
    }
}
