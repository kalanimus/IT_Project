using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
  public class ReviewService : IReviewService
  {
    private readonly IReviewRepository _reviewRepository;
    private readonly IUserRepository _userRepository;
    private readonly int ReviewPrice;
    private readonly int ActivityRatingReward;

    public ReviewService(IReviewRepository reviewRepository,
          IUserRepository userRepository,
          IConfiguration configuration)
    {
      _reviewRepository = reviewRepository;
      _userRepository = userRepository;
      ReviewPrice = configuration.GetValue<int>("Constants:ReviewPrice");
      ActivityRatingReward = configuration.GetValue<int>("Constants:ActivityRating");
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
      if (author.Balance < ReviewPrice) throw new InsufficientBalanceException();
      if (teacher == null) throw new Exception("Teacher not found");
      teacher.Rating = teacher.Rating == 0? review.Rating :(teacher.Rating + review.Rating) / 2;
      review.Teacher = teacher;
      author.Balance -= ReviewPrice;
      author.ActivityRate += ActivityRatingReward;

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

    public async Task<ModelReview> GetLatestAsync()
    {
      return await _reviewRepository.GetLatestAsync();
    }

    public async Task LikeReviewAsync(int reviewId, string username)
    {
      var review = await _reviewRepository.GetByIdAsync(reviewId);
      if (review == null) throw new Exception("Review not found");

      if (!review.LikedByUsernames.Contains(username))
      {
        review.LikedByUsernames.Add(username);
        review.DislikedByUsernames.Remove(username); // убираем дизлайк, если был
        await _reviewRepository.UpdateAsync(review);
      }
    }

    public async Task DislikeReviewAsync(int reviewId, string username)
    {
      var review = await _reviewRepository.GetByIdAsync(reviewId);
      if (review == null) throw new Exception("Review not found");

      if (!review.DislikedByUsernames.Contains(username))
      {
        review.DislikedByUsernames.Add(username);
        review.LikedByUsernames.Remove(username); // убираем лайк, если был
        await _reviewRepository.UpdateAsync(review);
      }
    }

    public async Task<(List<ModelReview> Reviews, int Total)> GetPagedByTeacherFullNameAsync(string fullName, int page, int pageSize)
    {
      return await _reviewRepository.GetPagedByTeacherFullNameAsync(fullName, page, pageSize);
    }
  }
}