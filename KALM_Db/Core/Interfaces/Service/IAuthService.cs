namespace Core.Interfaces;
public interface IAuthService
{
    Task<string> AuthenticateAsync(string username, string password);
    Task<string> RegisterAsync(string username, string password, int role);
}