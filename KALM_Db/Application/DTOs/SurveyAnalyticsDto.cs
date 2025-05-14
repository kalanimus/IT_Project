namespace Application.DTOs;

public class SurveyAnalyticsDto
{
    public int SurveyId { get; set; }
    public int Total { get; set; }
    public List<SurveyAnswerDto> Answers { get; set; }
}