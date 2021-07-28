using System.Threading;
using System.Threading.Tasks;
using API.Commands;
using Common.Entity;
using Common.Interfaces;
using Common.Notifications;
using MassTransit;
using MediatR;

namespace API.Handlers.Users
{
    public class CreateAppUserCommandHandler : IRequestHandler<CreateAppUserCommand, AppUser>
    {
        private readonly IUserRepository _userRepository;

        public CreateAppUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<AppUser> Handle(CreateAppUserCommand request, CancellationToken cancellationToken)
        {
            return await _userRepository.AddUser(request.AppUser);
        }
    }
}