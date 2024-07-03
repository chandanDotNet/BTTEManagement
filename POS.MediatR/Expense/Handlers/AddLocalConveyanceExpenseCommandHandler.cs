using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.MediatR.CommandAndQuery;
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

namespace BTTEM.MediatR.Handler
{
    public class AddLocalConveyanceExpenseCommandHandler : IRequestHandler<AddLocalConveyanceExpenseCommand, ServiceResponse<LocalConveyanceExpenseDto>>
    {

        private readonly IUserRoleRepository _userRoleRepository;
        private readonly UserInfoToken _userInfoToken;
        private readonly IMasterExpenseRepository _masterExpenseRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddLocalConveyanceExpenseCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;
        private readonly ILocalConveyanceExpenseRepository _localConveyanceExpenseRepository;

        public AddLocalConveyanceExpenseCommandHandler(
            IMasterExpenseRepository masterExpenseRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<AddLocalConveyanceExpenseCommandHandler> logger,
            IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper,
            UserInfoToken userInfoToken,
            IUserRoleRepository userRoleRepository,
            ILocalConveyanceExpenseRepository localConveyanceExpenseRepository)
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
        }

        public async Task<ServiceResponse<LocalConveyanceExpenseDto>> Handle(AddLocalConveyanceExpenseCommand request, CancellationToken cancellationToken)
        {
           // Guid LoginUserId = Guid.Parse(_userInfoToken.Id);

            var entity = _mapper.Map<LocalConveyanceExpense>(request);
            entity.Id = Guid.NewGuid();           
          
            _localConveyanceExpenseRepository.Add(entity);

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving Master Expense");
                return ServiceResponse<LocalConveyanceExpenseDto>.Return500();
            }

            var industrydto = _mapper.Map<LocalConveyanceExpenseDto>(entity);
            return ServiceResponse<LocalConveyanceExpenseDto>.ReturnResultWith200(industrydto);


        }

        }
    }
