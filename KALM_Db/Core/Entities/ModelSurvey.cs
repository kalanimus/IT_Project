using System;

namespace Core.Entities;

public class ModelSurvey
{
  public int Id { get; set; }
  public string Title { get; set; }
  public string Description { get; set; }
  public bool IsStandart { get; set; }
  public string QuestionsJson { get; set; }
  public ModelGroupTeacher Teacher { get; set; }
  public List<ModelSurveyAnswer> Results { get; set; }
}
