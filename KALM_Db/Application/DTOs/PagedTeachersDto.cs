namespace Application.DTOs;

public class TeacherDto
{
  public string FullName { get; set; }
  public double Rating { get; set; }
}
public class PagedTeachersDto
{
  public List<TeacherDto> Teachers { get; set; }
  public int Total { get; set; }
  public int Page { get; set; }
  public int PageSize { get; set; }
}