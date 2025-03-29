using Core.Entities;
namespace Core.Interfaces;
public interface ITokenService
{
    string GenerateToken(ModelUser user);
}