using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using AutoMapper;

namespace API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public AuthController(
            IAuthService authService,
            IUserService userService,
            IMapper mapper)
        {
            _authService = authService;
            _userService = userService;
            _mapper = mapper;
        }

        // POST: api/auth/login
        /// <summary>
        /// Аутентификация пользователя
        /// </summary>
        /// <param name="request">Логин и пароль</param>
        /// <returns>JWT-токен</returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequestDto request)
        {
            var token = await _authService.AuthenticateAsync(request.Username, request.Password);
            return Ok(new AuthResponseDto
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddHours(1)
            });
        }

        // POST: api/auth/register
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterRequestDto request)
        {
            var token = await _authService.RegisterAsync(request.Username, request.Password);
            if (token == null){
                return Ok(new AuthResponseDto{
                    Message = $"Письмо с подтверждением отправлено на почту! ({request.Username}@student.bmstu.ru)",
                    RequireVerification = true
                });
            }
            return Ok(new AuthResponseDto
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddHours(1),
                RequireVerification = false
            });
        }

        // POST: api/auth/verify
        [AllowAnonymous]
        [HttpPost("verify")]
        public async Task<ActionResult<VerifyResponseDto>> VerifyCode([FromBody] VerifyRequestDto request)
        {
            var token = await _authService.VerifyCodeAsync(request.Username, request.Password, request.Code);
            return Ok(new VerifyResponseDto
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddHours(1),
            });
        }

        // GET: api/auth/me (защищенный endpoint)
        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (userId == null)
                return Unauthorized("Invalid token claims");
            var user = await _userService.GetByIdAsync(userId);

            return _mapper.Map<UserDto>(user);
        }
    }
}