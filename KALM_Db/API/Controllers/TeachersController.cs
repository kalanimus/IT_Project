using Application.DTOs;
using Application.Services;
using AutoMapper;
using Core.Interfaces;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TeachersController : ControllerBase
{
  private readonly IReviewService _reviewService;
  private readonly IMapper _mapper;

  public TeachersController(IReviewService reviewService, IMapper mapper)
  {
    _reviewService = reviewService;
    _mapper = mapper;
  }

  [HttpPost("/reviews")]
  public async Task<ActionResult> PostReview([FromBody] ReviewDto reviewDto)
  {
    var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

    var review = _mapper.Map<ModelReview>(reviewDto);
    await _reviewService.PostReviewAsync(review, userName);
    return Ok("Review psoted successfully");
  }
}