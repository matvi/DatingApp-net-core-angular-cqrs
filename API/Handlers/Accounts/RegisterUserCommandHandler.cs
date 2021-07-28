using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using API.Commands;
using API.Data;
using Common.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Handlers.Accounts
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AppUser>
    {
        private readonly DataContext _dataContext;

        public RegisterUserCommandHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<AppUser> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            request.UserDto.UserName.ToLower();

            if(await UserExists(request.UserDto.UserName))
            {
                return null;
            }
            
            using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                UserName = request.UserDto.UserName,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.UserDto.Password)),
                PasswordSalt = hmac.Key
            };
            await _dataContext.Users.AddAsync(user);
            await _dataContext.SaveChangesAsync();

            return user;
        }
        
        private async Task<bool> UserExists(string userName)
        {
            return await _dataContext.Users.AnyAsync(u => u.UserName == userName);
        }
    }
}