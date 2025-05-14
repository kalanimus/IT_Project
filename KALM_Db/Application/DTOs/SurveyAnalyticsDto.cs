namespace Application.DTOs;

public class SurveyAnalitycsDto
{
    public int SurveyId { get; set; }
    public int Total { get; set; }
    public List<AnswerParamDto> Params { get; set; }
}