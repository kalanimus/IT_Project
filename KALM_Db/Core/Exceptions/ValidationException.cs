using Core.Exceptions;

public class ValidationException : AppException
{
  public ValidationException(string error, string message, string? details = null)
      : base(error, message, 400, details){}
}