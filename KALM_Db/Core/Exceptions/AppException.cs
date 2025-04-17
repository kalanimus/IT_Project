namespace Core.Exceptions;

public class AppException : Exception
{
  public string Error { get; }
  public int StatusCode { get; }
  public string? Details { get; }

  public AppException(string error, string message, int statusCode = 500, string? details = null)
      : base(message)
  {
    Error = error;
    StatusCode = statusCode;
    Details = details;
  }
}