using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class SurveyAnswerRepository : ISurveyAnswerRepository
{
    private readonly AppDbContext _context;

    public SurveyAnswerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ModelSurveyAnswer> GetByIdAsync(int id)
    {
        return await _context.SurveyAnswers
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<ModelSurveyAnswer>> GetAllAsync()
    {
        return await _context.SurveyAnswers
            .ToListAsync();
    }

    public async Task AddAsync(ModelSurveyAnswer answer)
    {
        await _context.SurveyAnswers.AddAsync(answer);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ModelSurveyAnswer answer)
    {
        _context.SurveyAnswers.Update(answer);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var answer = await _context.SurveyAnswers.FindAsync(id);
        if (answer != null)
        {
            _context.SurveyAnswers.Remove(answer);
            await _context.SaveChangesAsync();
        }
    }
}