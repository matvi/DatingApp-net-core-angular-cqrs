using Common.Dtos;
using Common.Entity;
using MediatR;

namespace API.Commands
{
    public class RegisterUserCommand : IRequest<AppUser>
    {
        public UserDto UserDto { get; set; }
    }
}