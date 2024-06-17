namespace CouncilsManagmentSystem.Models
{
    public class AuthenticationResault
    {
        public string Token { get; set; }
        public bool Result { get; set; }
        public List<string> Errors { get; set; }
    }
}
