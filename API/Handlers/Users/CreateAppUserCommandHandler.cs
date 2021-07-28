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
        private readonly IPublishEndpoint _publishEndpoint;

        public CreateAppUserCommandHandler(IUserRepository userRepository, IPublishEndpoint publishEndpoint)
        {
            _userRepository = userRepository;
            _publishEndpoint = publishEndpoint;
        }
        public async Task<AppUser> Handle(CreateAppUserCommand request, CancellationToken cancellationToken)
        {
            var appUserResult = await _userRepository.AddUser(request.AppUser);
            var appUserCreatedCommand = new AppUserCreatedCommand
            {
                UserId = appUserResult.AppUserId
            };

            await _publishEndpoint.Publish<AppUserCreatedCommand>(appUserCreatedCommand);

            return appUserResult;
        }
    }
}