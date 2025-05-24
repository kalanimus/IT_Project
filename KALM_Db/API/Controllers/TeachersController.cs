using Application.DTOs;
using Application.Services;
using AutoMapper;
using Core.Interfaces;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace API.Controllers
{
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

    [HttpGet("reviews")]
    public async Task<ActionResult> GetReviews([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
      if (page < 1) page = 1;
      if (pageSize < 1) pageSize = 10;

      var (reviews, total) = await _reviewService.GetPagedAsync(page, pageSize);
      var reviewsDto = _mapper.Map<List<ReviewDto>>(reviews);

      return Ok(new
      {
        Reviews = reviewsDto,
        Total = total,
        Page = page,
        PageSize = pageSize
      });
    }

    [HttpPost("reviews")]
    public async Task<ActionResult> PostReview([FromBody] ReviewDto reviewDto)
    {
      var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

      var review = _mapper.Map<ModelReview>(reviewDto);
      await _reviewService.PostReviewAsync(review, userName);
      return Ok("Review posted successfully");
    }
  }
}