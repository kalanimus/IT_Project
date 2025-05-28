using Core.Entities;

namespace Core.Interfaces;

public interface IReviewService : IService<ModelReview>
{
  Task PostReviewAsync(ModelReview review, string authorUsername);
  Task<(List<ModelReview> Reviews, int Total)> GetPagedAsync(int page, int pageSize);
  Task<ModelReview> GetLatestAsync();
  Task LikeReviewAsync(int reviewId, string username);
  Task DislikeReviewAsync(int reviewId, string username);
  Task<(List<ModelReview> Reviews, int Total)> GetPagedByTeacherFullNameAsync(string fullName, int page, int pageSize);

}
