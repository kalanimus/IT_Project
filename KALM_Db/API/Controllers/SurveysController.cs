using Microsoft.AspNetCore.Mvc;
using Core.Entities;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Core.Interfaces;
using Application.DTOs;
using AutoMapper;
using System.Security.Claims;

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

        public SurveysController(ISurveyService surveyService, ISurveyAnswerService surveyAnswerService, IMapper mapper)
        {
            _surveyService = surveyService;
            _surveyAnswerService = surveyAnswerService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize (Roles = "Студент, Преподаватель")]
        public async Task<ActionResult<SurveyResponseDto>> GetSurveys() {
          var userRole = User.Claims.FirstOrDefault (c => c.Type == ClaimTypes.Role)?.Value;
          var userName = User.Claims.FirstOrDefault (c => c.Type == ClaimTypes.Name)?.Value;
          var surveys = await _surveyService.GetByUserNameAsync (userName);

          var surveysDto = _mapper.Map<List<SurveyDto>>(surveys);
          return Ok(new SurveyResponseDto{
            Surveys = surveysDto,
            Total = surveysDto.Count()
          });
        }
    }
}