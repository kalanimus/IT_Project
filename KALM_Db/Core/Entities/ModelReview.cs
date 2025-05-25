using System;

namespace Core.Entities;

public class ModelReview
{
  public int Id { get; set; }
  public int TeacherId { get; set; }
  public ModelUser Teacher { get; set; }
  public int? AuthorId { get; set; }
  public ModelUser? Author { get; set; }
  public int Rating { get; set; }
  public string Text { get; set; }
  public DateTime CreatedAt { get; set; }
  public bool IsAnonymous { get; set; }
  public List<string> LikedByUsernames { get; set; }
  public List<string> DislikedByUsernames { get; set; }
}
