namespace Application.DTOs;

public class QuestionAnalyticsDto
{
    public string Question { get; set; }
    public string[] Answers { get; set; }
    public int[] Counts { get; set; }
}

public class SurveyAnalyticsDto
{
    public int SurveyId { get; set; }
    public int Total { get; set; }
    public List<QuestionAnalyticsDto> QuestionsAnalytics { get; set; }
}