using Core.Entities;

namespace Core.Interfaces;

public interface IUserRepository : IRepository<ModelUser>
{
  Task<ModelUser> GetByUsernameAsync(string username);
  Task<ModelUser> GetByFullNameAsync(string FullName);
  Task<List<ModelUser>> GetTeachersAsync();
  Task<List<ModelUser>> GetTopRatedTeachersAsync(int count);
  Task<ModelUser> GetMostActiveStudentAsync();
  Task<(List<ModelUser> Teachers, int Total)> GetPagedTeachersAsync(
    int page, int pageSize, string search, double? minRating, double? maxRating);
}