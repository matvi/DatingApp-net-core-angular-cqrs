using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using API.Entity;

namespace API.Interfaces
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