using System;

namespace Core.Entities;

public class ModelSurveyPart
{
  public string Title { get; set; }
  public List<ModelQuestion> Questions{ get; set; }
}
