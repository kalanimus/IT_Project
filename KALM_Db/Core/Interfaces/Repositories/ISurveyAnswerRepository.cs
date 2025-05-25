using Core.Entities;

namespace Core.Interfaces;

public interface ISurveyAnswerRepository : IRepository<ModelSurveyAnswer>
{
  Task<List<ModelSurveyAnswer>> GetBySurveyIdAsync(int surveyId);
  Task<ModelSurveyAnswer> GetByDetails(int id, string targetTeacher, string subject, string authorUsername);
  Task<List<ModelSurveyAnswer>> GetBySurveyDetailsAsync(int surveyId, string subject, string targetTeacher, string group);
}