using Core.Entities;

namespace Core.Interfaces;

public interface IReviewRepository : IRepository<ModelReview>
{
  Task<ModelReview> GetLatestAsync();
}
