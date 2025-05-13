namespace Application.DTOs;

public class SurveyDto
{
    public int ?Id { get; set; }
    public string Author { get; set; }
    public string Group { get; set; }
    public string Subject { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsStandart { get; set; }
    public List<SurveyPartDto> QuestionsJson { get; set; }
}