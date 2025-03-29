using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class SubjectRepository : ISubjectRepository
{
    private readonly AppDbContext _context;

    public SubjectRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ModelSubject> GetByIdAsync(int id)
    {
        return await _context.Subjects
            .Include(s => s.GroupTeachers)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<ModelSubject> GetByNameAsync(string SubjectName)
    {
        return await _context.Subjects
            .FirstOrDefaultAsync(s => s.SubjectName == SubjectName);
    }

    public async Task<List<ModelSubject>> GetAllAsync()
    {
        return await _context.Subjects
            .Include(s => s.GroupTeachers)
            .ToListAsync();
    }

    public async Task AddAsync(ModelSubject subject)
    {
        await _context.Subjects.AddAsync(subject);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ModelSubject subject)
    {
        _context.Subjects.Update(subject);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var subject = await _context.Subjects.FindAsync(id);
        if (subject != null)
        {
            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();
        }
    }
}