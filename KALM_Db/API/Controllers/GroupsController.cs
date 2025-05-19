using Application.DTOs;
using AutoMapper;
using Core.Interfaces;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GroupsController : ControllerBase
{
    private readonly IGroupService _groupService;
    private readonly IMapper _mapper;

    public GroupsController(IGroupService groupService, IMapper mapper)
    {
        _groupService = groupService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult> AddGroup([FromBody] GroupDto groupDto)
    {
        await _groupService.AddAsync(_mapper.Map<ModelGroup>(groupDto));
        return Ok("Group added successfully");
    }

    [HttpPost("{groupId}/add-student/{studentId}")]
    public async Task<ActionResult> AddStudentToGroup(int groupId, int studentId)
    {
        await _groupService.AddStudentToGroupAsync(groupId, studentId);
        return Ok("Student added to group successfully");
    }

    [HttpPost("{groupId}/add-teacher")]
    public async Task<ActionResult> AddTeacherToGroup(int groupId, [FromBody] GroupTeacherDto groupTeacherDto)
    {
        groupTeacherDto.GroupId = groupId;
        await _groupService.AddTeacherToGroupAsync(_mapper.Map<ModelGroupTeacher>(groupTeacherDto));
        return Ok("Teacher added to group successfully");
    }
}