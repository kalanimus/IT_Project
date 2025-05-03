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
        public IActionResult GetSurveys() {
          var userRole = User.Claims.FirstOrDefault (c => c.Type == ClaimTypes.Role)?.Value;
          return userRole switch
          {
            "Студент" => Ok(new { Message = "Секретные данные для Студента", Data = "ConfidentialInfo" }),
            "Преподаватель" => Ok(new { Message = "Обычные данные для Препода", Data = "PublicInfo" }),
            _ => Ok(new {claims = userRole}) // Если роль не подходит (хотя Authorize уже проверил)
          };
        }
    }
}