namespace Application.DTOs;

public class ReviewDto
{
  public string TeacherFullName { get; set; }
  public string? AuthorFullName { get; set; }
  public int Rating { get; set; }
  public string Text { get; set; }
  public DateTime CreatedAt { get; set; }
  public bool IsAnonymous { get; set; }
}
