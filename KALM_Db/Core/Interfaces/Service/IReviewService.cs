using Core.Entities;

namespace Core.Interfaces;

public interface IReviewService : IService<ModelReview>
{
  Task PostReviewAsync(ModelReview review, string authorUsername);
}
