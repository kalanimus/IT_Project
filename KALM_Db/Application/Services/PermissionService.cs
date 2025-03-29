using Core.Entities;
using Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;

        public PermissionService(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<ModelPermission> GetByIdAsync(int id)
        {
            return await _permissionRepository.GetByIdAsync(id);
        }

        public async Task<List<ModelPermission>> GetAllAsync()
        {
            return await _permissionRepository.GetAllAsync();
        }

        public async Task AddAsync(ModelPermission permission)
        {
            await _permissionRepository.AddAsync(permission);
        }

        public async Task UpdateAsync(ModelPermission permission)
        {
            var existingPermission = await _permissionRepository.GetByIdAsync(permission.Id);
            if (existingPermission == null) throw new Exception("Permission not found");

            // Обновляем поля существующего разрешения
            existingPermission.PermissionName = permission.PermissionName;
            // Другие поля, если необходимо

            await _permissionRepository.UpdateAsync(existingPermission);
        }

        public async Task DeleteAsync(int id)
        {
            var permission = await _permissionRepository.GetByIdAsync(id);
            if (permission == null) throw new Exception("Permission not found");

            await _permissionRepository.DeleteAsync(permission.Id);
        }
    }
}