namespace Application.DTOs;

public class SurveyDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsStandart { get; set; }
    public string QuestionsJson { get; set; }
}