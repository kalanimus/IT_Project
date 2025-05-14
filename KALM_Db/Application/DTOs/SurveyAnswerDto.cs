namespace Application.DTOs;

public class SurveyAnswerDto
{
    public int SurveyId { get; set; }
    public string ?TargetTeacher { get; set; }
     public string ?Group { get; set; }
    public string ?Subject { get; set; }
    public List<QuestionAnswerDto> Answers { get; set; }
}