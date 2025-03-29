using Core.Entities;
using Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<ModelRole> GetByIdAsync(int id)
        {
            return await _roleRepository.GetByIdAsync(id);
        }

        public async Task<List<ModelRole>> GetAllAsync()
        {
            return await _roleRepository.GetAllAsync();
        }

        public async Task AddAsync(ModelRole role)
        {
            await _roleRepository.AddAsync(role);
        }

        public async Task UpdateAsync(ModelRole role)
        {
            var existingRole = await _roleRepository.GetByIdAsync(role.Id);
            if (existingRole == null) throw new Exception("Role not found");

            // Обновляем поля существующей роли
            existingRole.RoleName = role.RoleName;
            // Другие поля, если необходимо

            await _roleRepository.UpdateAsync(existingRole);
        }

        public async Task DeleteAsync(int id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null) throw new Exception("Role not found");

            await _roleRepository.DeleteAsync(role.Id);
        }
    }
}