namespace CouncilsManagmentSystem.DTOs
{
    public class AddResultToTopicDto
    {
        public int CouncilId { get; set; }
        public int TopicId { get; set; }
        public string Result { get; set; }
    }
}