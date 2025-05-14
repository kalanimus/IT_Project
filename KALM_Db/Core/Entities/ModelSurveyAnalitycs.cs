namespace Core.Entities;

public class ModelAnswerParam
{
  public string Param { get; set; }
  public int Count { get; set; }
}

public class ModelSurveyAnalytics
{
  public int SurveyId { get; set; }
  public List<ModelAnswerParam> Params { get; set; }
}
