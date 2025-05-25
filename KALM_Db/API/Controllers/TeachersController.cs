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
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public TeachersController(
      IReviewService reviewService,
      IMapper mapper,
      IUserService userService)
    {
      _reviewService = reviewService;
      _mapper = mapper;
      _userService = userService;
    }

    [HttpGet("reviews")]
    public async Task<ActionResult<PagedReviewsDto>> GetReviews([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
      if (page < 1) page = 1;
      if (pageSize < 1) pageSize = 10;

      var (reviews, total) = await _reviewService.GetPagedAsync(page, pageSize);
      var reviewsDto = _mapper.Map<List<ReviewDto>>(reviews);

      return Ok(new PagedReviewsDto
      {
        Reviews = reviewsDto,
        Total = total,
        Page = page,
        PageSize = pageSize
      });
    }

    [HttpGet("reviews/latest")]
    public async Task<ActionResult> GetLatestReviews()
    {
      var review = await _reviewService.GetLatestAsync();
      var reviewsDto = _mapper.Map<ReviewDto>(review);

      return Ok(reviewsDto);
    }

    [HttpPost("reviews")]
    public async Task<ActionResult> PostReview([FromBody] ReviewDto reviewDto)
    {
      var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

      var review = _mapper.Map<ModelReview>(reviewDto);
      await _reviewService.PostReviewAsync(review, userName);
      return Ok("Review posted successfully");
    }

    /// <summary>
    /// Поставить лайк отзыву.
    /// </summary>
    [HttpPost("reviews/{reviewId}/like")]
    public async Task<IActionResult> LikeReview(int reviewId)
    {
      var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
      if (string.IsNullOrEmpty(userName))
        return Unauthorized();

      await _reviewService.LikeReviewAsync(reviewId, userName);
      return Ok("Лайк учтен");
    }

    /// <summary>
    /// Поставить дизлайк отзыву.
    /// </summary>
    [HttpPost("reviews/{reviewId}/dislike")]
    public async Task<IActionResult> DislikeReview(int reviewId)
    {
      var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
      if (string.IsNullOrEmpty(userName))
        return Unauthorized();

      await _reviewService.DislikeReviewAsync(reviewId, userName);
      return Ok("Дизлайк учтен");
    }

    /// <summary>
    /// Получить список преподавателей с пагинацией.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<PagedTeachersDto>> GetTeachers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
      if (page < 1) page = 1;
      if (pageSize < 1) pageSize = 10;

      // Получаем пользователей с ролью "Преподаватель"
      var (teachers, total) = await _userService.GetPagedTeachersAsync(page, pageSize);
      var teachersDto = _mapper.Map<List<TeacherDto>>(teachers);

      return Ok(new PagedTeachersDto
      {
        Teachers = teachersDto,
        Total = total,
        // Teachers = null,
        // Total = 0,
        Page = page,
        PageSize = pageSize
      });
      return Ok();
    }
  }
}