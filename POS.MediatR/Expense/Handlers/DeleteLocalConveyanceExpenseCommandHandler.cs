﻿using AutoMapper;
using BTTEM.MediatR.Commands;
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
    public class DeleteLocalConveyanceExpenseCommandHandler : IRequestHandler<DeleteLocalConveyanceExpenseCommand, ServiceResponse<bool>>
    {

        private readonly IUserRoleRepository _userRoleRepository;
        private readonly UserInfoToken _userInfoToken;
        private readonly IMasterExpenseRepository _masterExpenseRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteLocalConveyanceExpenseCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;
        private readonly ILocalConveyanceExpenseRepository _localConveyanceExpenseRepository;

        public DeleteLocalConveyanceExpenseCommandHandler(
            IMasterExpenseRepository masterExpenseRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<DeleteLocalConveyanceExpenseCommandHandler> logger,
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

        public async Task<ServiceResponse<bool>> Handle(DeleteLocalConveyanceExpenseCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _localConveyanceExpenseRepository.FindAsync(request.Id);
            if (entityExist == null)
            {
                _logger.LogError("Expense does not exists.");
                return ServiceResponse<bool>.Return409("Expense does not exists.");
            }

            _localConveyanceExpenseRepository.Delete(request.Id);

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving Expense.");
                return ServiceResponse<bool>.Return500();
            }

            return ServiceResponse<bool>.ReturnResultWith200(true);

        }

        }
    }
