using Core.Entities;
namespace Core.Interfaces;

public interface IStudentsParser
{
  Task<List<ModelGroupStudent>> ParseStudentsAsync(Stream fileStream);
}
