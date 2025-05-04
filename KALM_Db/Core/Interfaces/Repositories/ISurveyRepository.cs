using Core.Entities;

namespace Core.Interfaces;

public interface ISurveyRepository : IRepository<ModelSurvey> {
  Task<List<ModelSurvey>> GetByUserNameAsync(string userName);
  Task<List<ModelSurvey>> GetByGroupAsync(int groupNumber);
}