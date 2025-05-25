using Core.Entities;

namespace Core.Interfaces;

public interface ISurveyService : IService<ModelSurvey> {
  Task<List<ModelSurvey>> GetByUserNameAsync(string userName);
  Task<ModelSurveyAnalytics> GetAnalyticsAsync(int surveyId, string groupName, string subjectName, string userName);

  
}
