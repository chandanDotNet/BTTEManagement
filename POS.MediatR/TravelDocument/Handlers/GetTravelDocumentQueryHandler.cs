using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Handlers;
using BTTEM.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Handlers
{
    public class GetTravelDocumentQueryHandler : IRequestHandler<GetTravelDocumentQuery, List<TravelDocumentDto>> 
    {

        private readonly ITravelDocumentRepository _travelDocumentRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<GetTravelDocumentQueryHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;

        public GetTravelDocumentQueryHandler(
            ITravelDocumentRepository travelDocumentRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<GetTravelDocumentQueryHandler> logger,
            IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper)
        {
            _travelDocumentRepository = travelDocumentRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
        }


        public async Task<List<TravelDocumentDto>> Handle(GetTravelDocumentQuery request, CancellationToken cancellationToken)
        {
            var expense = await _travelDocumentRepository.All.Where(a=>a.UserId==a.UserId).ToListAsync();
            if (expense == null)
            {
                _logger.LogError("Expense not found");
                //return Task<List<TravelDocumentDto>>.Return404();
            }
            var expenseDto = _mapper.Map<List<TravelDocumentDto>>(expense);
           // return Task<List<TravelDocumentDto>>.ReturnResultWith200(expenseDto);
           return expenseDto;
        }
    }
}
