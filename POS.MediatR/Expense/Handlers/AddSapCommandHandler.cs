using AutoMapper;
using BTTEM.Data.Entities.Expense;
using BTTEM.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Data;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MediatR;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class AddSapCommandHandler : IRequestHandler<AddSapCommand, ServiceResponse<SapDto>>
    {
        private readonly ISapRepository _sapRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddSapCommandHandler> _logger;
        private readonly IUserRepository _userRepository;
        private readonly UserInfoToken _userInfoToken;

        public AddSapCommandHandler(
            ISapRepository sapRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<AddSapCommandHandler> logger,
            IUserRepository userRepository,
           UserInfoToken userInfoToken)
        {
            _sapRepository = sapRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _userRepository = userRepository;
            _userInfoToken = userInfoToken;
        }

        public async Task<ServiceResponse<SapDto>> Handle(AddSapCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Sap>(request);

            _sapRepository.Add(entity);

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving Expense");
                return ServiceResponse<SapDto>.Return500();
            }
            var sapDto = _mapper.Map<SapDto>(entity);
             return ServiceResponse<SapDto>.ReturnResultWith200(sapDto);
        }

    }
}
