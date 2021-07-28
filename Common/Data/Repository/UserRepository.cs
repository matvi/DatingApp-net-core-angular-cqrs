using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Dtos;
using Common.Entity;
using Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Common.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _databaseContext;
        private readonly IMapper _mapper;

        public UserRepository(DataContext databaseContext, IMapper mapper)
        {
            _databaseContext = databaseContext;
            _mapper = mapper;
        }

        public async Task<AppUser> AddUser(AppUser appUser)
        {
            var result = await _databaseContext.AddAsync(appUser);
            return appUser;
        }

        public async Task<IEnumerable<MemberDto>> GetAllMembersDtoAsync()
        {
            var result = await _databaseContext.Users.
                ProjectTo<MemberDto>(_mapper.ConfigurationProvider).ToListAsync();

            return result;
        }

        public async Task<IEnumerable<AppUser>> GetAllUsersAsync()
        {
            return await _databaseContext.Users.Include(p => p.Photos).ToListAsync();
        }

        public async Task<MemberDto> GetMemberDtoByUserName(string username)
        {
            var result = await _databaseContext.Users
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(u => u.UserName == username.ToLower());
            
            return result;
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _databaseContext.Users.Include(p => p.Photos).SingleOrDefaultAsync(u => u.AppUserId == id);
        }

        public async Task<AppUser> GetUserByUserName(string username) 
            => await _databaseContext.Users.Include(p => p.Photos).SingleOrDefaultAsync(u => u.UserName == username);

        public async Task<bool> SaveAllAsync()
        {
            return (await _databaseContext.SaveChangesAsync()) > 0;
        }

        public void Update(AppUser appUser)
        {
            _databaseContext.Entry(appUser).State = EntityState.Modified;
        }
    }
}