using Application.DTOs;
using Application.Services;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
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
}