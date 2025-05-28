using Core.Entities;

namespace Core.Interfaces;

public interface IReviewRepository : IRepository<ModelReview>
{
  Task<ModelReview> GetLatestAsync();
  Task<(List<ModelReview> Reviews, int Total)> GetPagedByTeacherFullNameAsync(string fullName, int page, int pageSize);
}
