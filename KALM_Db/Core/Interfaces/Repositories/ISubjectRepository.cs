using Core.Entities;

namespace Core.Interfaces;

public interface ISubjectRepository : IRepository<ModelSubject> 
{
  Task<ModelSubject> GetByNameAsync(string SubjectName);

}
