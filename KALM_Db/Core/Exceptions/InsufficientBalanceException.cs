using Core.Exceptions;

public class InsufficientBalanceException : AppException
{
  public InsufficientBalanceException()
      : base("Not enouh balance", "Не достаточно монет", 402){}
}