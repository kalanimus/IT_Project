using Core.Entities;

namespace Core.Interfaces;

public interface IUserRepository : IRepository<ModelUser> {
  Task<ModelUser> GetByUsernameAsync(string username);
  Task<ModelUser> GetByFullNameAsync(string FullName);

}