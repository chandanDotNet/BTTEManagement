using AutoMapper;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, List<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetAllUserQueryHandler(
           IUserRepository userRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<UserDto>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            var entities = await _userRepository.All.Include(d => d.CompanyAccounts).Include(c => c.Grades).Include(b => b.Departments).Include(u=>u.UserRoles).ThenInclude(a => a.Role).ToListAsync();
            return _mapper.Map<List<UserDto>>(entities);
        }
    }
}
