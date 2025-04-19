using Core.Entities;

namespace Core.Interfaces;

public interface IUserService : IService<ModelUser> {
    Task UploadStudentsAsync(Stream fileStream);
 }