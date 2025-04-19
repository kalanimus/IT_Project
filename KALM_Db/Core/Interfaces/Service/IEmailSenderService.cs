namespace Core.Interfaces;

public interface IEmailSenderService
{
    Task SendEmail(string email, int Code);
}