using Core.Entities;

namespace Core.Interfaces;

public interface IUserRepository : IRepository<ModelUser>
{
  Task<ModelUser> GetByUsernameAsync(string username);
  Task<ModelUser> GetByFullNameAsync(string FullName);
  Task<List<ModelUser>> GetTeachersAsync();
  Task<List<ModelUser>> GetTopRatedTeachersAsync(int count);
  Task<ModelUser> GetMostActiveStudentAsync();
}