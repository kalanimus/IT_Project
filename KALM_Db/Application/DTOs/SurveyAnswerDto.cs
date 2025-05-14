namespace Application.DTOs;

public class SurveyAnswerDto
{
    public int SurveyId { get; set; }
    public List<AnswerParamDto> Params { get; set; }
}