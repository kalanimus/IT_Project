using Core.Interfaces;

namespace Application.Services;
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepo;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher _hasher;

    public AuthService(
        IUserRepository userRepo,
        ITokenService tokenService,
        IPasswordHasher hasher)
    {
        _userRepo = userRepo;
        _tokenService = tokenService;
        _hasher = hasher;
    }

    public async Task<string> AuthenticateAsync(string username, string password)
    {
        var user = await _userRepo.GetByUsernameAsync(username);
        if (user == null || !_hasher.Verify(password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid credentials");

        return _tokenService.GenerateToken(user);
    }

    public async Task<string> RegisterAsync(string username, string password, int role)
    {
        var user = await _userRepo.GetByUsernameAsync(username);
        if (user == null || user.PasswordHash != null) 
        {
            throw new UnauthorizedAccessException("Invalid Username or user already registred");
        }

        user.PasswordHash = _hasher.Hash(password);
        await _userRepo.UpdateAsync(user);
        return _tokenService.GenerateToken(user);
    }

}