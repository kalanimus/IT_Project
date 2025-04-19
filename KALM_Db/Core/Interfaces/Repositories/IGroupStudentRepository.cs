using Core.Entities;

namespace Core.Interfaces;

public interface IGroupStudentRepository 
{
  Task<ModelGroupStudent> GetByIdsAsync(int groupId, int studentId);
  Task<List<ModelGroupStudent>> GetAllAsync();
  Task AddAsync(ModelGroupStudent entity);
  Task UpdateAsync(ModelGroupStudent entity);
  Task DeleteAsync(int groupId, int studentId);
}
