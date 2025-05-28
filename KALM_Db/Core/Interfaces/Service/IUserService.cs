using Core.Entities;

namespace Core.Interfaces;

public interface IUserService : IService<ModelUser>
{
    Task UploadStudentsAsync(Stream fileStream);
    Task<ModelUser> GetByUsernameAsync(string username);
    Task<(List<ModelUser> Teachers, int Total)> GetPagedTeachersAsync(
    int page, int pageSize, string search, double? minRating, double? maxRating);
    Task<List<ModelUser>> GetTopRatedTeachersAsync(int count);
    Task<ModelUser> GetMostActiveStudentAsync();
    Task<double> GetTeachersRatingAsync(string fullName);
 }