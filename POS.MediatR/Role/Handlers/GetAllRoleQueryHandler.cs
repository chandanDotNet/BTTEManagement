using AutoMapper;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using POS.Data;
using System.Linq;

namespace POS.MediatR.Handlers
{
    public class GetAllRoleQueryHandler : IRequestHandler<GetAllRoleQuery, List<RoleDto>>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllRoleQueryHandler> _logger;

        public GetAllRoleQueryHandler(
           IRoleRepository roleRepository,
            IMapper mapper,
            ILogger<GetAllRoleQueryHandler> logger)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<List<RoleDto>> Handle(GetAllRoleQuery request, CancellationToken cancellationToken)
        {
            var entities = await _roleRepository.All.Include(c => c.UserRoles).ToListAsync();
            var entitiesDto = entities.Select(c =>
            {
                return new RoleDto
                {
                    Id = c.Id,
                    Description = c.Description,
                    Name = c.Name,
                    IsActive = c.IsActive.Value,
                    NoOfUsers = c.UserRoles.Count,
                };
            }).ToList();
            return entitiesDto;
            //return _mapper.Map<List<RoleDto>>(entities);
        }
    }
}
