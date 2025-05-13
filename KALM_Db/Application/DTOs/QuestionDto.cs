using System.Text.Json.Serialization;

namespace Application.DTOs;

public class QuestionDto
{
 public string Type { get; set; }
 public string Text { get; set; }
 public string[] ?Options { get; set; }
}
