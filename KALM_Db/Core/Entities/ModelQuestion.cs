using System.Text.Json.Serialization;

namespace Core.Entities;

public enum QuestionType
{
  multiple_choice,
  single_choice,
  text,
  rating
}

public class ModelQuestion
{
[JsonConverter(typeof(JsonStringEnumConverter))]
 public QuestionType Type { get; set; }
 public string Text { get; set; }
 public string[] ?Options { get; set; }
}
