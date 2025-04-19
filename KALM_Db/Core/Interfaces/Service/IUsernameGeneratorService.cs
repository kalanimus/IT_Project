namespace Core.Interfaces;

public interface IUsernameGeneratorService
{
  string GenerateTeacherUsername(string fullName);
  string GenerateStudentUsername(string fullName, string CCNumber);
}