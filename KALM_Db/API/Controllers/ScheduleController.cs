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
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;
        private readonly IMapper _mapper;

        public ScheduleController(IScheduleService scheduleService, IMapper mapper)
        {
            _scheduleService = scheduleService;
            _mapper = mapper;
        }

        [HttpPost("csv")]
        public async Task<IActionResult> UploadScheduleCsv(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Файл не был загружен");

            if (!Path.GetExtension(file.FileName).Equals(".csv", StringComparison.OrdinalIgnoreCase))
                return BadRequest("Поддерживаются только CSV-файлы");
            await using var stream = file.OpenReadStream(); // Преобразование IFormFile в Stream
            try
            {
                await _scheduleService.UploadScheduleAsync(stream);
                return Ok("Файл успешно обработан");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка обработки файла: {ex.Message}");
            }

        }
    }
}