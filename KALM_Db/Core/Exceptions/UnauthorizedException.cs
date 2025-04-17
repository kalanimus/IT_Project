using Core.Exceptions;

public class UnauthorizedException : AppException
{
  public UnauthorizedException(string error, string message)
      : base(error, message, 401){}
}