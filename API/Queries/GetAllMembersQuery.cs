using System;
using System.Collections.Generic;
using Common.Dtos;
using Common.Interfaces;
using MediatR;

namespace Common.Queries
{
    public class GetAllMembersQuery : IRequest<IEnumerable<MemberDto>>, ICachable
    {
        public string CacheKey { get; set; } = "GetAllMembersQuery";
    }
}