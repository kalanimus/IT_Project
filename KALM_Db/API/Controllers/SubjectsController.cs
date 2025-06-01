using System.Security.Claims;
using Application.DTOs;
using Application.Services;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubjectsController : ControllerBase
{
    private readonly ISubjectService _subjectService;
    private readonly IMapper _mapper;

    public SubjectsController(ISubjectService subjectService, IMapper mapper)
    {
        _subjectService = subjectService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult> AddSubject([FromBody] SubjectDto subjectDto)
    {
        await _subjectService.AddAsync(_mapper.Map<ModelSubject>(subjectDto));
        return Ok("Subject added successfully");
    }

    [HttpGet]
    [Authorize(Roles = "Администратор, Преподаватель")]
    public async Task<ActionResult<IEnumerable<SubjectDto>>> GetAllGroups()
    {
        var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        var subjects = new List<ModelSubject>();
        if (userRole == "Преподаватель")
        {
            subjects = await _subjectService.GetSubjectsByTeacherUserNameAsync(userName);
            if (subjects == null)
            {
                return NotFound("Group not found for the teacher");
            }
        }
        else if (userRole == "Администратор")
        {
            subjects = await _subjectService.GetAllAsync();
            if (subjects == null || !subjects.Any())
            {
                return NotFound("No groups found");
            }
        }

        var subjectDtos = _mapper.Map<IEnumerable<SubjectDto>>(subjects);
        return Ok(subjectDtos);
    }
}