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


}