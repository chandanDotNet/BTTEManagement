using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.MediatR.Command;
using BTTEM.MediatR.Expense.Commands;
using BTTEM.MediatR.Handlers;
using BTTEM.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Handlers
{
    public class GetAllTravelDeskQueryHandler : IRequestHandler<GetAllTravelDeskQuery, List<TravelDeskExpenseDto>>
    {

        private readonly ITravelDeskExpenseRepository _travelDeskExpenseRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllTravelDeskQueryHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;

        public GetAllTravelDeskQueryHandler(
            ITravelDeskExpenseRepository travelDeskExpenseRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<GetAllTravelDeskQueryHandler> logger,
            IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper)
        {
            _travelDeskExpenseRepository = travelDeskExpenseRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
        }

        public async Task<List<TravelDeskExpenseDto>> Handle(GetAllTravelDeskQuery request, CancellationToken cancellationToken)
        {
            List < TravelDeskExpenseDto > listData =new List<TravelDeskExpenseDto> ();
            if (request.Id.HasValue)
            {
                return listData;
            }
            var entities = await _travelDeskExpenseRepository.All.ToListAsync();
            var dtoEntities = _mapper.Map<List<TravelDeskExpenseDto>>(entities);
            return dtoEntities;

        }

    }
}
