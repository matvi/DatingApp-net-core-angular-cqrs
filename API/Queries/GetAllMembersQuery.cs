using System;
using System.Collections.Generic;
using Common.Dtos;
using MediatR;

namespace Common.Queries
{
    public class GetAllMembersQuery : IRequest<IEnumerable<MemberDto>>
    {

    }
}