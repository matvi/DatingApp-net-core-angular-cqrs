using Common.Entity;

namespace Common.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}