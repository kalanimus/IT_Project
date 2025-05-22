namespace Core.Interfaces;

public interface IMistralService
{
  Task<string> SendPromptAsync(string prompt);
}