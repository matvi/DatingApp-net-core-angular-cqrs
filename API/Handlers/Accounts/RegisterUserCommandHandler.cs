using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using API.Commands;
using API.Data;
using Common.Entity;
using Common.Notifications;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Handlers.Accounts
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AppUser>
    {
        private readonly DataContext _dataContext;
        private readonly IPublishEndpoint _publishEndpoint;

        public RegisterUserCommandHandler(DataContext dataContext, IPublishEndpoint publishEndpoint)
        {
            _dataContext = dataContext;
            _publishEndpoint = publishEndpoint;
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
            var userRegistered = await _dataContext.Users.AddAsync(user);
            await _dataContext.SaveChangesAsync();
            
            var appUserCreatedCommand = new RegisterUserCreatedCommand
            {
                UserId = userRegistered.Entity.AppUserId
            };
            await _publishEndpoint.Publish<RegisterUserCreatedCommand>(appUserCreatedCommand);

            return user;
        }
        
        private async Task<bool> UserExists(string userName)
        {
            return await _dataContext.Users.AnyAsync(u => u.UserName == userName);
        }
    }
}