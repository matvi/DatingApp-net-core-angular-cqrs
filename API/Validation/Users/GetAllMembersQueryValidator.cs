using Common.Queries;
using FluentValidation;

namespace API.Validation.Users
{
    public class GetAllMembersQueryValidator : AbstractValidator<GetAllMembersQuery>
    {
        public GetAllMembersQueryValidator()
        {
        }
    }
}