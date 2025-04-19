using Core.Interfaces;

using Application.DTOs;
using Core.Exceptions;
using System.Security.Cryptography.X509Certificates;

namespace Application.Services;
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepo;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher _hasher;
    private readonly IEmailSenderService _emailSenderService;

    public AuthService(
        IUserRepository userRepo,
        ITokenService tokenService,
        IPasswordHasher hasher,
        IEmailSenderService emailSenderService)
    {
        _userRepo = userRepo;
        _tokenService = tokenService;
        _hasher = hasher;
        _emailSenderService = emailSenderService;
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

        if(user.Role.RoleName != "Студент") {
            user.PasswordHash = _hasher.Hash(password);
            await _userRepo.UpdateAsync(user);
            return _tokenService.GenerateToken(user);
        } else {
            var code = Random.Shared.Next(100000, 999999);
            user.VerificationCode = code;
            await _userRepo.UpdateAsync(user);
            await _emailSenderService.SendEmail(user.Email, code);
            return null;
        }
    }

    public async Task<string> VerifyCodeAsync(string username, string password, int code){
        var user = await _userRepo.GetByUsernameAsync(username);
        if (user == null) throw new ValidationException("user_do_not_exsists", "Пользователя не существует");
        else if (user.VerificationCode == null) throw new ValidationException("no_code_here", "Код пуст");
        else if (user.VerificationCode == code)
        {
            user.PasswordHash = _hasher.Hash(password);
            await _userRepo.UpdateAsync(user);
            return _tokenService.GenerateToken(user);
        } else {throw new ValidationException("invalid code", "Неправильный код");}
    }

}