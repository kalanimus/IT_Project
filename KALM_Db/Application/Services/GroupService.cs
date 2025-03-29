using Core.Entities;
using Core.Interfaces;

namespace Application.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;

        public GroupService(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<ModelGroup> GetByIdAsync(int id)
        {
            return await _groupRepository.GetByIdAsync(id);
        }

        public async Task<List<ModelGroup>> GetAllAsync()
        {
            return await _groupRepository.GetAllAsync();
        }

        public async Task AddAsync(ModelGroup group)
        {
            await _groupRepository.AddAsync(group);
        }

        public async Task UpdateAsync(ModelGroup group)
        {
            var existingGroup = await _groupRepository.GetByIdAsync(group.Id);
            if (existingGroup == null) throw new Exception("Group not found");

            // Обновляем поля существующей группы
            existingGroup.Group_Name = group.Group_Name;
            // Другие поля, если необходимо

            await _groupRepository.UpdateAsync(existingGroup);
        }

        public async Task DeleteAsync(int id)
        {
            var group = await _groupRepository.GetByIdAsync(id);
            if (group == null) throw new Exception("Group not found");

            await _groupRepository.DeleteAsync(group.Id);
        }
    }
}