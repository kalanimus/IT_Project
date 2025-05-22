using Core.Entities;
using Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
  public class ReviewService : IReviewService
  {
    private readonly IReviewRepository _reviewRepository;
    private readonly IUserRepository _userRepository;

    public ReviewService(IReviewRepository reviewRepository,
          IUserRepository userRepository)
    {
      _reviewRepository = reviewRepository;
      _userRepository = userRepository;
    }

    public async Task<ModelReview> GetByIdAsync(int id)
    {
      return await _reviewRepository.GetByIdAsync(id);
    }

    public async Task<List<ModelReview>> GetAllAsync()
    {
      return await _reviewRepository.GetAllAsync();
    }

    public async Task AddAsync(ModelReview review)
    {
      await _reviewRepository.AddAsync(review);
    }

    public async Task UpdateAsync(ModelReview review)
    {
      var existingReview = await _reviewRepository.GetByIdAsync(review.Id);
      if (existingReview == null) throw new Exception("Review not found");

      existingReview.Author = review.Author;
      existingReview.AuthorId = review.AuthorId;
      existingReview.Teacher = review.Teacher;
      existingReview.TeacherId = review.TeacherId;
      existingReview.CreatedAt = review.CreatedAt;
      existingReview.IsAnonymous = review.IsAnonymous;
      existingReview.Rating = review.Rating;
      existingReview.Text = review.Text;

      await _reviewRepository.UpdateAsync(existingReview);
    }

    public async Task DeleteAsync(int id)
    {
      var review = await _reviewRepository.GetByIdAsync(id);
      if (review == null) throw new Exception("Review not found");

      await _reviewRepository.DeleteAsync(review.Id);
    }

    public async Task PostReviewAsync(ModelReview review, string authorUsername)
    {
      var teacher = await _userRepository.GetByFullNameAsync(review.Teacher.FullName);
      var author = await _userRepository.GetByUsernameAsync(authorUsername);

      if (teacher == null) throw new Exception("Teacher not found");
      teacher.Rating = (teacher.Rating + review.Rating) / 2;
      review.Teacher = teacher;

      if (review.IsAnonymous)
      {
        review.Author = null;
      }
      else
      {
        review.Author = author;
      }
      await _reviewRepository.AddAsync(review);
    }

    public async Task<(List<ModelReview> Reviews, int Total)> GetPagedAsync(int page, int pageSize)
    {
      var all = await _reviewRepository.GetAllAsync();
      var total = all.Count;
      var paged = all
          .OrderByDescending(r => r.CreatedAt)
          .Skip((page - 1) * pageSize)
          .Take(pageSize)
          .ToList();
      return (paged, total);
    }
  }
}