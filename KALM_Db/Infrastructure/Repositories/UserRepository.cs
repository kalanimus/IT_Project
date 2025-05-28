using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
  private readonly AppDbContext _context;

  public UserRepository(AppDbContext context)
  {
    _context = context;
  }

  public async Task<ModelUser> GetByIdAsync(int id)
  {
    return await _context.Users
      .Include(u => u.Role)
      .FirstOrDefaultAsync(u => u.Id == id);
  }

  public async Task<ModelUser> GetByUsernameAsync(string username)
  {
    return await _context.Users
      .Include(u => u.Role)
      .FirstOrDefaultAsync(u => u.Username == username);
  }

  public async Task<ModelUser> GetByFullNameAsync(string FullName)
  {
    return await _context.Users
      .Include(u => u.Role)
      .FirstOrDefaultAsync(u => u.FullName == FullName);
  }

  public async Task<List<ModelUser>> GetAllAsync()
  {
    return await _context.Users
      .Include(u => u.Role)
      .ToListAsync();
  }

  public async Task AddAsync(ModelUser user)
  {
    await _context.Users.AddAsync(user);
    await _context.SaveChangesAsync();
  }
  public async Task UpdateAsync(ModelUser user)
  {
    _context.Users.Update(user);
    await _context.SaveChangesAsync();
  }

  public async Task DeleteAsync(int id)
  {
    var user = await _context.Users.FindAsync(id);
    if (user != null)
    {
      _context.Users.Remove(user);
      await _context.SaveChangesAsync();
    }
  }

  public async Task<List<ModelUser>> GetTeachersAsync()
  {
    return await _context.Users
    .Include(u => u.Role)
    .Where(u => u.Role.RoleName == "Преподаватель")
    .ToListAsync();
  }

  public async Task<List<ModelUser>> GetTopRatedTeachersAsync(int count)
  {
    return await _context.Users
        .Where(u => u.RoleId == 4) // 4 — Преподаватель
        .OrderByDescending(u => u.Rating) // Предполагается, что есть поле Rating
        .Take(count)
        .ToListAsync();
  }

  public async Task<ModelUser> GetMostActiveStudentAsync()
  {
    // Предполагается, что у студента RoleId == 3 и есть поле ActivityRating
    return await _context.Users
        .Where(u => u.RoleId == 3) // 3 — студент
        .OrderByDescending(u => u.ActivityRate) // поле активности
        .FirstOrDefaultAsync();
  }

  public async Task<(List<ModelUser> Teachers, int Total)> GetPagedTeachersAsync(
    int page, int pageSize, string search, double? minRating, double? maxRating)
{
    var query = _context.Users.AsQueryable();

    // Только преподаватели
    query = query.Where(u => u.RoleId == 4);

    // Поиск по имени/фамилии
    if (!string.IsNullOrWhiteSpace(search))
        query = query.Where(u => u.FullName.Contains(search));

    // Фильтр по рейтингу
    if (minRating.HasValue)
        query = query.Where(u => u.Rating >= minRating.Value);
    if (maxRating.HasValue)
        query = query.Where(u => u.Rating <= maxRating.Value);

    var total = await query.CountAsync();

    var teachers = await query
        .OrderByDescending(u => u.Rating)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return (teachers, total);
}
}