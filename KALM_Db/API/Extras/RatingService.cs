using Application.DTOs;
using AutoMapper;
using Core.Interfaces;

namespace API.Extras;

public interface IRatingService
{
    Task CalculateRatingAsync(SurveyAnswerDto answers);
}
public class RatingService : IRatingService
{
  private readonly IGroupTeacherService _groupTeacherService;
  private readonly IUserService _userService;

  public RatingService(
      IGroupTeacherService groupTeacherService,
      IUserService userService,
      ISurveyAnswerService surveyAnswerService,
      ISurveyService surveyService,
      IMapper mapper)
  {
      _groupTeacherService = groupTeacherService;
      _userService = userService;
  }

  public async Task CalculateRatingAsync(SurveyAnswerDto answers)
  {
      var teacher = await _groupTeacherService.GetByDetailsAsync(answers.Group, answers.Subject, answers.TargetTeacher);
      if (teacher == null)
      {
          throw new Exception("Teacher not found");
      }

      float rating = 0;

      for(var i = 0; i < 4; i++)
      {
          rating += int.Parse(answers.Answers[i].Answer);
      }

      rating /= 4;
      
      if (teacher.Teacher.Rating == 0)
      {
        teacher.Teacher.Rating = rating;
      }
      else
      {
        teacher.Teacher.Rating = (teacher.Teacher.Rating + rating) / 2;
      }

      await _userService.UpdateAsync(teacher.Teacher);
  }
}