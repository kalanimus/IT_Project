using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PermissionRepository : IPermissionRepository
{
    private readonly AppDbContext _context;

    public PermissionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ModelPermission> GetByIdAsync(int id)
    {
        return await _context.Permissions
            .Include(p => p.PermissionsForRoles)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<ModelPermission>> GetAllAsync()
    {
        return await _context.Permissions
            .Include(p => p.PermissionsForRoles)
            .ToListAsync();
    }

    public async Task AddAsync(ModelPermission permission)
    {
        await _context.Permissions.AddAsync(permission);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ModelPermission permission)
    {
        _context.Permissions.Update(permission);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var permission = await _context.Permissions.FindAsync(id);
        if (permission != null)
        {
            _context.Permissions.Remove(permission);
            await _context.SaveChangesAsync();
        }
    }
}