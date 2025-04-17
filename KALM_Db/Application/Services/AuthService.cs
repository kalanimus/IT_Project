using Core.Interfaces;
using Core.Exceptions;

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
        if (user == null) throw new ValidationException("user_do_not_exsists", "Пользователя не существует");
        else if (user.PasswordHash == null) throw new UnauthorizedException("user_is_not_registred", "Пользователь не зарегистрирован");
        else if (!_hasher.Verify(password, user.PasswordHash)) throw new UnauthorizedException("wrong_passwrod", "Не верный пароль");

        return _tokenService.GenerateToken(user);
    }

    public async Task<string> RegisterAsync(string username, string password)
    {
        var user = await _userRepo.GetByUsernameAsync(username);
        if (user == null) throw new ValidationException("user_do_not_exsists", "Пользователя не существует");
        else if (user.PasswordHash != null) throw new ValidationException("user_is_already_registred", "Пользователь уже зарегистрирован");

        user.PasswordHash = _hasher.Hash(password);
        await _userRepo.UpdateAsync(user);
        return _tokenService.GenerateToken(user);
    }

}