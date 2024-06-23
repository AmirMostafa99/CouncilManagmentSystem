namespace CouncilsManagmentSystem.DTOs
{
    public class ReportRequestDto
    {
        public int CouncilId { get; set; }
        public List<int> TopicIds { get; set; }
    }
}
