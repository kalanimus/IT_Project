using Core.Entities;

namespace Core.Interfaces;

public interface ISurveyAnswerRepository : IRepository<ModelSurveyAnswer> 
{
  Task<List<ModelSurveyAnswer>> GetBySurveyIdAsync(int surveyId);
}