using Core.Entities;

namespace Core.Interfaces;

public interface IRating 
{
  Task CalculateRatingAsync(int surveyId, int groupId, int subjectId, int teacherId);
}