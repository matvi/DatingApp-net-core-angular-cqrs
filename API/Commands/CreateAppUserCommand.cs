using Common.Entity;
using MediatR;

namespace API.Commands
{
    public class CreateAppUserCommand : IRequest<AppUser>
    {
        public AppUser AppUser { get; set; }
        
        public CreateAppUserCommand(AppUser appUser)
        {
            AppUser = appUser;
        }
    }
}