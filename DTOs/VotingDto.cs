namespace CouncilsManagmentSystem.DTOs
{
    public class VotingDto
    {
        public int VoteId { get; set; }
        public string VoteType { get; set; } // Should be "agree", "disagree", or "abstaining"
    }
}
