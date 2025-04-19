using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class GroupStudentRepository : IGroupStudentRepository
{
    private readonly AppDbContext _context;

    public GroupStudentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ModelGroupStudent> GetByIdsAsync(int groupId, int studentId)
    {
        return await _context.GroupStudents
            .Include(g => g.Group)
            .Include(g => g.Student)
            .FirstOrDefaultAsync(g => g.Group.Id == groupId && g.Student.Id == studentId);
    }

    public async Task<List<ModelGroupStudent>> GetAllAsync()
    {
        return await _context.GroupStudents
            .Include(g => g.Group)
            .Include(g => g.Student)
            .ToListAsync();
    }

    public async Task AddAsync(ModelGroupStudent student)
    {
        await _context.GroupStudents.AddAsync(student);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ModelGroupStudent student)
    {
        _context.GroupStudents.Update(student);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int groupId, int teacherId)
    {
      var student = await _context.GroupStudents
      .FirstOrDefaultAsync(gt => gt.Group.Id == groupId 
                              && gt.Student.Id == teacherId);
  
      if (student != null)
      {
          _context.GroupStudents.Remove(student);
          await _context.SaveChangesAsync();
      }
    }
}