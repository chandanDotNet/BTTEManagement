using AutoMapper;
using BTTEM.MediatR.Commands;
using BTTEM.MediatR.Expense.Commands;
using BTTEM.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Handlers
{
    public class DeleteCarBikeLogBookExpenseDocumentCommandHandler : IRequestHandler<DeleteCarBikeLogBookExpenseDocumentCommand, ServiceResponse<bool>>
    {

        private readonly IUserRoleRepository _userRoleRepository;
        private readonly UserInfoToken _userInfoToken;
        private readonly IMasterExpenseRepository _masterExpenseRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteCarBikeLogBookExpenseDocumentCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;
        private readonly ILocalConveyanceExpenseRepository _localConveyanceExpenseRepository;
        private readonly ILocalConveyanceExpenseDocumentRepository _localConveyanceExpenseDocumentRepository;
        private readonly ICarBikeLogBookExpenseDocumentRepository _carBikeLogBookExpenseDocumentRepository;
        private readonly ICarBikeLogBookExpenseTollParkingDocumentRepository _carBikeLogBookExpenseTollParkingDocumentRepository;
        private readonly ICarBikeLogBookExpenseRefillingDocumentRepository _carBikeLogBookExpenseRefillingDocumentRepository;

        public DeleteCarBikeLogBookExpenseDocumentCommandHandler(
            IMasterExpenseRepository masterExpenseRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<DeleteCarBikeLogBookExpenseDocumentCommandHandler> logger,
            IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper,
            UserInfoToken userInfoToken,
            IUserRoleRepository userRoleRepository,
            ILocalConveyanceExpenseRepository localConveyanceExpenseRepository,
            ILocalConveyanceExpenseDocumentRepository localConveyanceExpenseDocumentRepository,
            ICarBikeLogBookExpenseDocumentRepository carBikeLogBookExpenseDocumentRepository,
            ICarBikeLogBookExpenseTollParkingDocumentRepository carBikeLogBookExpenseTollParkingDocumentRepository,
            ICarBikeLogBookExpenseRefillingDocumentRepository carBikeLogBookExpenseRefillingDocumentRepository)
        {
            _masterExpenseRepository = masterExpenseRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
            _userInfoToken = userInfoToken;
            _userRoleRepository = userRoleRepository;
            _localConveyanceExpenseRepository = localConveyanceExpenseRepository;
            _localConveyanceExpenseDocumentRepository = localConveyanceExpenseDocumentRepository;
            _carBikeLogBookExpenseDocumentRepository = carBikeLogBookExpenseDocumentRepository;
            _carBikeLogBookExpenseTollParkingDocumentRepository = carBikeLogBookExpenseTollParkingDocumentRepository;
            _carBikeLogBookExpenseRefillingDocumentRepository = carBikeLogBookExpenseRefillingDocumentRepository;
        }

        public async Task<ServiceResponse<bool>> Handle(DeleteCarBikeLogBookExpenseDocumentCommand request, CancellationToken cancellationToken)
        {
            var entityDocumentExist = await _carBikeLogBookExpenseDocumentRepository.FindAsync(request.Id);            
            var entityRefillingExist = await _carBikeLogBookExpenseRefillingDocumentRepository.FindAsync(request.Id);
            var entityTollParkingExist = await _carBikeLogBookExpenseTollParkingDocumentRepository.FindAsync(request.Id);

            if (entityDocumentExist == null && entityTollParkingExist == null && entityRefillingExist == null)
            {
                _logger.LogError("Expense does not exists.");
                return ServiceResponse<bool>.Return409("Expense does not exists.");
            }

            if (entityDocumentExist != null)
            {
                _carBikeLogBookExpenseDocumentRepository.Remove(entityDocumentExist);                
            }
            if (entityRefillingExist != null)
            {
                _carBikeLogBookExpenseRefillingDocumentRepository.Remove(entityRefillingExist);
            }
            if (entityTollParkingExist != null)
            {
                _carBikeLogBookExpenseTollParkingDocumentRepository.Remove(entityTollParkingExist);
            }

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving Expense.");
                return ServiceResponse<bool>.Return500();
            }

            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
