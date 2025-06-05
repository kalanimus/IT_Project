using Microsoft.AspNetCore.Mvc;
using Core.Entities;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Core.Interfaces;
using Application.DTOs;
using AutoMapper;
using System.Security.Claims;
using API.Extras;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SurveysController : ControllerBase
    {
        private readonly ISurveyService _surveyService;
        private readonly ISurveyAnswerService _surveyAnswerService;
        private readonly IMapper _mapper;
        private readonly IGroupTeacherService _groupTeacherService;
        private readonly IRatingService _ratingService;

        public SurveysController(
            ISurveyService surveyService,
            ISurveyAnswerService surveyAnswerService,
            IMapper mapper,
            IGroupTeacherService groupTeacherService,
            IRatingService ratingService)
        {
            _surveyService = surveyService;
            _surveyAnswerService = surveyAnswerService;
            _mapper = mapper;
            _groupTeacherService = groupTeacherService;
            _ratingService = ratingService;
        }

        [HttpGet]
        [Authorize(Roles = "Студент, Преподаватель")]
        public async Task<ActionResult<SurveyResponseDto>> GetSurveys()
        {
            var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var surveys = await _surveyService.GetByUserNameAsync(userName);

            var surveysDto = _mapper.Map<List<SurveyDto>>(surveys);
            if (User.IsInRole("Студент"))
            {
                foreach (var s in surveysDto)
                {
                    // Устанавливаем статус завершенности опроса
                    s.IsCompleted = await _surveyAnswerService.IsSurveyCompletedAsync((int)s.Id, s.Author, s.Subject, userName);
                }
            }

            return Ok(new SurveyResponseDto
            {
                Surveys = surveysDto,
                Total = surveysDto.Count()
            });
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Студент, Преподаватель")]
        public async Task<ActionResult<SurveyDto>> GetSurveyById(int id)
        {
            var survey = await _surveyService.GetByIdAsync(id);
            if (survey == null)
            {
                return NotFound($"Survey with ID {id} not found.");
            }

            var surveyDto = _mapper.Map<SurveyDto>(survey);
            return Ok(surveyDto);
        }

        [HttpPost]
        [Authorize(Roles = "Преподаватель")]
        public async Task<ActionResult> CreateSurvey([FromBody] SurveyDto surveyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Выполняем асинхронный поиск до маппинга
            var groupTeacher = await _groupTeacherService.GetByDetailsAsync(surveyDto.Group, surveyDto.Subject, surveyDto.Author);
            if (groupTeacher == null)
            {
                return BadRequest("Некорректные значения для группы, предмета или преподавателя.");
            }

            // Выполняем маппинг
            var survey = _mapper.Map<ModelSurvey>(surveyDto);
            survey.Teacher = groupTeacher;

            await _surveyService.AddAsync(survey);

            return CreatedAtAction(nameof(GetSurveyById), new { id = survey.Id }, surveyDto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Преподаватель")]
        public async Task<ActionResult> UpdateSurvey(int id, [FromBody] SurveyDto surveyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingSurvey = await _surveyService.GetByIdAsync(id);
            if (existingSurvey == null)
            {
                return NotFound($"Survey with ID {id} not found.");
            }

            var survey = _mapper.Map<ModelSurvey>(surveyDto);
            survey.Id = id; // Ensure the ID is preserved
            await _surveyService.UpdateAsync(survey);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Преподаватель")]
        public async Task<ActionResult> DeleteSurvey(int id)
        {
            var existingSurvey = await _surveyService.GetByIdAsync(id);
            if (existingSurvey == null)
            {
                return NotFound($"Survey with ID {id} not found.");
            }

            await _surveyService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/complete")]
        [Authorize(Roles = "Студент")]
        public async Task<ActionResult> CompleteSurvey(int id, [FromBody] SurveyAnswerDto surveyAnswerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingSurvey = await _surveyService.GetByIdAsync(id);
            if (existingSurvey == null)
            {
                return NotFound($"Survey with ID {id} not found.");
            }
            var AuthorUsername = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var surveyAnswer = _mapper.Map<ModelSurveyAnswer>(surveyAnswerDto);
            surveyAnswer.SurveyId = id;
            surveyAnswer.AuthorUsername = AuthorUsername;

            if (existingSurvey.IsStandart)
            {
                var teacher = await _groupTeacherService.GetByDetailsAsync(
                    surveyAnswerDto.Group,
                    surveyAnswerDto.Subject,
                    surveyAnswerDto.TargetTeacher);
                if (teacher == null)
                {
                    return BadRequest("Некорректные значения для группы, предмета или преподавателя.");
                }

                await _ratingService.CalculateRatingAsync(surveyAnswerDto);
                await _surveyAnswerService.AddAsync(surveyAnswer);
                return Ok("Ответ успешно сохранен и рейтинг обновлен.");
            }

            await _surveyAnswerService.AddAsync(surveyAnswer);

            return Ok();
        }

        [HttpGet("analytics")]
        [Authorize(Roles = "Преподаватель")]
        public async Task<ActionResult<SurveyAnalyticsDto>> GetSurveyAnalytics(
            [FromQuery] int id,
            [FromQuery] string group,
            [FromQuery] string subject)
        {
            var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var survey = await _surveyService.GetByIdAsync(id);
            if (survey == null)
            {
                return NotFound($"Survey with ID {id} not found.");
            }

            var analytics = await _surveyService.GetAnalyticsAsync(id, group, subject, userName);
            Console.WriteLine(analytics.Params.Count);
            var analyticsDto = _mapper.Map<SurveyAnalyticsDto>(analytics);

            return Ok(analyticsDto);
        }
    }
}