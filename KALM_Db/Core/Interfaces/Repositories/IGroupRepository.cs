using Core.Entities;

namespace Core.Interfaces;

public interface IGroupRepository : IRepository<ModelGroup> 
{
  Task<ModelGroup> GetByGroupNameAsync(string GroupName);
}
