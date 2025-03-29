using Core.Entities;

namespace Core.Interfaces;

public interface IGroupTeacherRepository 
{
  Task<ModelGroupTeacher> GetByIdsAsync(int groupId, int teacherId, int subjectId);
  Task<List<ModelGroupTeacher>> GetAllAsync();
  Task AddAsync(ModelGroupTeacher entity);
  Task UpdateAsync(ModelGroupTeacher entity);
  Task DeleteAsync(int groupId, int teacherId, int subjectId);
}
