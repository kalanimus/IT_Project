namespace Core.Entities;

public class ModelAnswerParam
{
  public string Param { get; set; }
  public string QuestionType { get; set; }
  public int Count { get; set; }
  public double Average { get; set; }
  public Dictionary<string, int> AnswerCounts { get; set; } // Для одного/нескольких ответов
  public List<string> TextAnswers { get; set; } // Для открытых вопросов
}

public class ModelSurveyAnalytics
{
  public int SurveyId { get; set; }
  public List<ModelAnswerParam> Params { get; set; }
  public string GeneralComment { get; set; } // Комментарий от Мистраля
}
