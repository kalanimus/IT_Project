namespace Core.Interfaces;

public interface IUsernameGeneratorService
{
  public string Generate(string fullName);
}