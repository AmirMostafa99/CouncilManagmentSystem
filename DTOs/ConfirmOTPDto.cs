using Org.BouncyCastle.Bcpg.OpenPgp;

namespace CouncilsManagmentSystem.DTOs
{
    public class ConfirmOTPDto
    {
        public string Token { get; set; }
        public int OTP { get; set; }
    }
}
