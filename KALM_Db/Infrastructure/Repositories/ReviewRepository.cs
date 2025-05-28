using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Infrastructure.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly AppDbContext _context;

    public ReviewRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ModelReview> GetByIdAsync(int id)
    {
        return await _context.Reviews
            .Include(g => g.Teacher)
            .Include(g => g.Author)
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<List<ModelReview>> GetAllAsync()
    {
        return await _context.Reviews
            .ToListAsync();
    }

    public async Task AddAsync(ModelReview review)
    {
        await _context.Reviews.AddAsync(review);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ModelReview Review)
    {
        _context.Reviews.Update(Review);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var Review = await _context.Reviews.FindAsync(id);
        if (Review != null)
        {
            _context.Reviews.Remove(Review);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<ModelReview> GetLatestAsync()
    {
        return await _context.Reviews
            .Include(g => g.Teacher)
            .Include(g => g.Author)
            .OrderByDescending(g => g.CreatedAt)
            .FirstOrDefaultAsync();
    }

    public async Task<(List<ModelReview> Reviews, int Total)> GetPagedByTeacherFullNameAsync(string fullName, int page, int pageSize)
{
    // Предполагается, что у ModelReview есть навигационное свойство Teacher (ModelUser)
    // и у ModelUser есть FirstName и LastName
    var query = _context.Reviews
        .Where(r => (r.Teacher.FullName) == fullName);

    var total = await query.CountAsync();

    var reviews = await query
        .Include(r => r.Teacher)
        .Include(r => r.Author)
        .OrderByDescending(r => r.CreatedAt)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return (reviews, total);
}
}