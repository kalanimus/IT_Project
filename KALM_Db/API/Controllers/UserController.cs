using Microsoft.AspNetCore.Mvc;
using Core.Entities;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Core.Interfaces;
using Application.DTOs;
using AutoMapper;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();

            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetAllUsers()
        {
            var users = await _userService.GetAllAsync();
            if (users == null) return NotFound();

            var usersDto = _mapper.Map<List<UserDto>>(users);
            return Ok(usersDto);
        }

        // POST: api/users
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(UserDto userDto)
        {
            var user = _mapper.Map<ModelUser>(userDto);
            await _userService.AddAsync(user);

            var createdUserDto = _mapper.Map<UserDto>(user);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, createdUserDto);
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserDto userDto)
        {
            if (id != userDto.Id) return BadRequest();

            var user = _mapper.Map<ModelUser>(userDto);
            await _userService.UpdateAsync(user);

            return NoContent();
        }

        // DELETE: api/users/{id}
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }

        // POST: api/users/csv
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("csv")]
        public async Task<IActionResult> UploadStudents(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Файл не был загружен");

            if (!Path.GetExtension(file.FileName).Equals(".csv", StringComparison.OrdinalIgnoreCase))
                return BadRequest("Поддерживаются только CSV-файлы");
            await using var stream = file.OpenReadStream(); // Преобразование IFormFile в Stream
            try
            {
                await _userService.UploadStudentsAsync(stream);
                return Ok("Файл успешно обработан");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка обработки файла: {ex.Message}");
            }
        }
    }
}