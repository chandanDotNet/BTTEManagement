using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Expense.Handlers;
using BTTEM.Repository;
using MediatR;
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
    public class GetWalletQueryHandler : IRequestHandler<GetWalletQuery, ServiceResponse<WalletDto>>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<GetWalletQueryHandler> _logger;
        public GetWalletQueryHandler(
           IWalletRepository walletRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<GetWalletQueryHandler> logger
            )
        {
            _walletRepository = walletRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
        }

        public async Task<ServiceResponse<WalletDto>> Handle(GetWalletQuery request, CancellationToken cancellationToken)
        {
            var entity = await _walletRepository.All.Where(c => c.UserId == request.UserId).FirstOrDefaultAsync();
            if (entity != null)
                return ServiceResponse<WalletDto>.ReturnResultWith200(_mapper.Map<WalletDto>(entity));
            else
            {
                _logger.LogError("Not found");
                return ServiceResponse<WalletDto>.Return404();
            }
        }

    }
}
