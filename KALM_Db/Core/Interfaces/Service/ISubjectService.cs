using Core.Entities;

namespace Core.Interfaces;

public interface ISubjectService : IService<ModelSubject>
{
  Task<List<ModelSubject>> GetSubjectsByTeacherUserNameAsync(string username);
}
