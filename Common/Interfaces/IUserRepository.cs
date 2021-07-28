using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Dtos;
using Common.Entity;

namespace Common.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser appUser);

        Task<AppUser> GetUserByIdAsync(int id);

        Task<IEnumerable<AppUser>> GetAllUsersAsync();

        Task<bool> SaveAllAsync();

        Task<AppUser> GetUserByUserName(string username);

        Task<AppUser> AddUser(AppUser appUser);

        Task<IEnumerable<MemberDto>> GetAllMembersDtoAsync();

        Task<MemberDto> GetMemberDtoByUserName(string username);
    }
}