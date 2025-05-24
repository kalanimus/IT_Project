namespace Application.DTOs;

public class QuestionAnalyticsDto
{
    public string Question { get; set; }
    public string QuestionType { get; set; } 
    public int Count { get; set; }
    public double Average { get; set; }
    public Dictionary<string, int> AnswerCounts { get; set; }
    public List<string> TextAnswers { get; set; }
}

public class SurveyAnalyticsDto
{
    public int SurveyId { get; set; }
    public List<QuestionAnalyticsDto> Params { get; set; }
    public string GeneralComment { get; set; }
}