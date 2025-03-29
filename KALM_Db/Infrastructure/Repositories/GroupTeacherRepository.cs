using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class GroupTeacherRepository : IGroupTeacherRepository
{
    private readonly AppDbContext _context;

    public GroupTeacherRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ModelGroupTeacher> GetByIdsAsync(int groupId, int teacherId, int subjectId)
    {
        return await _context.GroupTeachers
            .Include(g => g.Group)
            .Include(g => g.Teacher)
            .Include(g => g.Subject)
            .FirstOrDefaultAsync(g => g.Group.Id == groupId && g.Teacher.Id == teacherId && g.Subject.Id == subjectId);
    }

    public async Task<List<ModelGroupTeacher>> GetAllAsync()
    {
        return await _context.GroupTeachers
            .Include(g => g.Group)
            .Include(g => g.Teacher)
            .Include(g => g.Subject)
            .ToListAsync();
    }

    public async Task AddAsync(ModelGroupTeacher teacher)
    {
        await _context.GroupTeachers.AddAsync(teacher);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ModelGroupTeacher teacher)
    {
        _context.GroupTeachers.Update(teacher);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int groupId, int teacherId, int subjectId)
    {
      var teacher = await _context.GroupTeachers
      .FirstOrDefaultAsync(gt => gt.Group.Id == groupId 
                              && gt.Teacher.Id == teacherId 
                              && gt.Subject.Id == subjectId);
  
      if (teacher != null)
      {
          _context.GroupTeachers.Remove(teacher);
          await _context.SaveChangesAsync();
      }
    }
}