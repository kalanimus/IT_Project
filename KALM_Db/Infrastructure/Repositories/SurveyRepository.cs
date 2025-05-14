using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class SurveyRepository : ISurveyRepository
{
    private readonly AppDbContext _context;

    public SurveyRepository(AppDbContext context)
    {
        _context = context;
    }

  public async Task<ModelSurvey> GetStandartAsync()
  {
    return await _context.Surveys.FirstOrDefaultAsync(p => p.IsStandart);
  }


    public async Task<ModelSurvey> GetByIdAsync(int id)
    {
        return await _context.Surveys
            .Include(s => s.Teacher)
            .ThenInclude(s => s.Teacher)
            .Include(s => s.Teacher)
            .ThenInclude(s => s.Group)
            .Include(s => s.Teacher)
            .ThenInclude(s => s.Subject)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
    public async Task<List<ModelSurvey>> GetByUserNameAsync(string userName){
        return await _context.Surveys.Where(p => p.Teacher.Teacher.Username == userName).ToListAsync();
    }

    public async Task<List<ModelSurvey>> GetByGroupAsync(int groupNumber){
        return await _context.Surveys.Where(p => p.Teacher.GroupId == groupNumber).ToListAsync();
    }


    public async Task<List<ModelSurvey>> GetAllAsync()
    {
        return await _context.Surveys
            .ToListAsync();
    }

    public async Task AddAsync(ModelSurvey survey)
    {
        await _context.Surveys.AddAsync(survey);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ModelSurvey survey)
    {
        _context.Surveys.Update(survey);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var survey = await _context.Surveys.FindAsync(id);
        if (survey != null)
        {
            _context.Surveys.Remove(survey);
            await _context.SaveChangesAsync();
        }
    }
}