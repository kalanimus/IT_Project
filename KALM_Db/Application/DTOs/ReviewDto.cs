namespace Application.DTOs;

public class ReviewDto
{
  public int Id { get; set; }
  public string TeacherFullName { get; set; }
  public string? AuthorFullName { get; set; }
  public int Rating { get; set; }
  public string Text { get; set; }
  public DateTime CreatedAt { get; set; }
  public bool IsAnonymous { get; set; }
  public List<string>? LikedByUsernames { get; set; }
  public List<string>? DislikedByUsernames { get; set; }
}

public class PagedReviewsDto
{
    public List<ReviewDto> Reviews { get; set; }
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}