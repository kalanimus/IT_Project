using Core.Exceptions;

public class ForbiddenException : AppException
{
  public ForbiddenException(string error, string message)
      : base(error, message, 403){}
}