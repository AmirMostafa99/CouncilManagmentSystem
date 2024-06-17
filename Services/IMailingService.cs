namespace CouncilsManagmentSystem.Services
{
    public interface IMailingService
    {
        Task SendEmailAsync(string mailto, string subject, string body);

    }
}
