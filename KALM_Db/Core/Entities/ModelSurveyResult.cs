using System;

namespace Core.Entities;

public class ModelSurveyAnswer
{
  public int Id { get; set; }
  public int SurveyId { get; set; }
  public ModelSurvey Survey { get; set; }
  public string AnswerJson { get; set; }

  
}
