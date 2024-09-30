using AutoMapper;
using BTTEM.Data.Dto;
using BTTEM.MediatR.AppVersionUpdate.Command;
using BTTEM.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.AppVersionUpdate.Handler
{
    public class GetAppVersionUpdateQueryHandler : IRequestHandler<GetAppVersionUpdateQuery, List<AppVersionUpdateDto>>
    {
        private readonly IAppVersionUpdateRepository _appVersionUpdateRepository;
        private readonly IMapper _mapper;

        public GetAppVersionUpdateQueryHandler(IAppVersionUpdateRepository appVersionUpdateRepository, IMapper mapper)
        {
            _appVersionUpdateRepository = appVersionUpdateRepository;
            _mapper = mapper;
        }
        public async Task<List<AppVersionUpdateDto>> Handle(GetAppVersionUpdateQuery request, CancellationToken cancellationToken)
        {
            var entities = await _appVersionUpdateRepository.All
               .Select(c => new AppVersionUpdateDto
               {
                   Id = c.Id,
                   AppType = c.AppType,
                   Version = c.Version
               }).ToListAsync();

            return entities;
        }
    }
}