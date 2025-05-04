namespace Application.DTOs;

// AuthResponseDto.cs
public class SurveyResponseDto
{
    public List<SurveyDto> Surveys { get; set; }
    public int Total { get; set; }
}