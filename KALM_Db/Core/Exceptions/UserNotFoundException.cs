using Core.Exceptions;

public class UserNotFoundException : AppException
{
  public UserNotFoundException()
      : base("User do not exists", "Пользователь не найден", 404){}
}