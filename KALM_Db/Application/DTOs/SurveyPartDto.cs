using System;

namespace Application.DTOs;

public class SurveyPartDto
{
  public string Title { get; set; }
  public List<QuestionDto> Questions{ get; set; }
}
