using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class GroupRepository : IGroupRepository
{
    private readonly AppDbContext _context;

    public GroupRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ModelGroup> GetByIdAsync(int id)
    {
        return await _context.Groups
            .Include(g => g.GroupStudents)
            .Include(g => g.GroupTeachers)
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<ModelGroup> GetByGroupNameAsync(string GroupName)
    {
        return await _context.Groups
            .FirstOrDefaultAsync(u => u.Group_Name == GroupName);
    }

    public async Task<List<ModelGroup>> GetAllAsync()
    {
        return await _context.Groups
            .Include(g => g.GroupStudents)
            .Include(g => g.GroupTeachers)
            .ToListAsync();
    }

    public async Task AddAsync(ModelGroup group)
    {
        await _context.Groups.AddAsync(group);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ModelGroup group)
    {
        _context.Groups.Update(group);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var group = await _context.Groups.FindAsync(id);
        if (group != null)
        {
            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
        }
    }
}