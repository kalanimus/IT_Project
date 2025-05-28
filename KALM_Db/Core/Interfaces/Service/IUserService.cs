using Core.Entities;

namespace Core.Interfaces;

public interface IUserService : IService<ModelUser>
{
    Task UploadStudentsAsync(Stream fileStream);
    Task<ModelUser> GetByUsernameAsync(string username);
    Task<(List<ModelUser> Teachers, int Total)> GetPagedTeachersAsync(int page, int pageSize);
    Task<List<ModelUser>> GetTopRatedTeachersAsync(int count);
    Task<ModelUser> GetMostActiveStudentAsync();
 }