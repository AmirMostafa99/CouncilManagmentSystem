using CouncilsManagmentSystem.Attributes;

namespace CouncilsManagmentSystem.DTOs
{
    public class TopicDto
    {
        public string? Title { get; set; }
        public string? Notes { get; set; }
        public string? Type { get; set; }
        public int CouncilId { get; set; }

        [AllowedExtensions(".pdf")]
        public IFormFile? Attachment { get; set; }
    }
}
