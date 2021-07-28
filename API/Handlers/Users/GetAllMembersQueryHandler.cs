using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Common.Dtos;
using Common.Interfaces;
using Common.Queries;
using MediatR;

namespace API.Handlers.Users
{
    public class GetAllMembersHandler : IRequestHandler<GetAllMembersQuery, IEnumerable<MemberDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetAllMembersHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<MemberDto>> Handle(GetAllMembersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllMembersDtoAsync();
            var members = _mapper.Map<IEnumerable<MemberDto>>(users);
            return members;
        }
    }
}