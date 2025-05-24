using Core.Entities;

namespace Core.Interfaces;

public interface ISurveyAnswerService : IService<ModelSurveyAnswer>
{
  Task<bool> IsSurveyCompletedAsync(int id, string TeacherFullName, string Subject, string userName);
}
