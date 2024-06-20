using CouncilsManagmentSystem.Migrations;

namespace CouncilsManagmentSystem.Models
{
    public class AuthenticationResault
    {
        public string Token { get; set; }
        public bool Result { get; set; }
        public object Permission { get; set; }
        public List<string> Errors { get; set; }
    }
}
