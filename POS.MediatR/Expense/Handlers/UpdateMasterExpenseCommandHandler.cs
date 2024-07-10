using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.Commands;
using BTTEM.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.MediatR.Handlers;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Handlers
{
    public class UpdateMasterExpenseCommandHandler : IRequestHandler<UpdateMasterExpenseCommand, ServiceResponse<bool>>
    {

        private readonly IMasterExpenseRepository _masterExpenseRepository;
        private readonly IGroupExpenseRepository _groupExpenseRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateMasterExpenseCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;

        public UpdateMasterExpenseCommandHandler(
            IMasterExpenseRepository masterExpenseRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<UpdateMasterExpenseCommandHandler> logger,
            IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper,
            IGroupExpenseRepository groupExpenseRepository)
        {
            _masterExpenseRepository = masterExpenseRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
            _groupExpenseRepository = groupExpenseRepository;
        }

        public async Task<ServiceResponse<bool>> Handle(UpdateMasterExpenseCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _masterExpenseRepository.FindAsync(request.Id);
            if (entityExist == null)
            {
                _logger.LogError("Expense does not exists.");
                return ServiceResponse<bool>.Return409("Expense does not exists.");
            }

            entityExist.Name = request.Name;

            if (!string.IsNullOrEmpty(request.Status))
            {
                entityExist.Status = request.Status;
            }
            if (!string.IsNullOrEmpty(request.ApprovalStage))
            {
                entityExist.ApprovalStage = request.ApprovalStage;
            }
            if (request.TotalAmount > 0)
            {
                entityExist.TotalAmount = request.TotalAmount;
            }
            if (request.ReimbursementAmount > 0)
            {
                entityExist.ReimbursementAmount = request.ReimbursementAmount;
            }
            if (request.AdvanceMoney > 0)
            {
                entityExist.AdvanceMoney = request.AdvanceMoney;
            }
            entityExist.NoOfBill = request.NoOfBill;

            _masterExpenseRepository.Update(entityExist);

            var groupExpenseExist = await _groupExpenseRepository.All.Where(v => v.MasterExpenseId == request.Id).ToListAsync();
            if (groupExpenseExist.Count > 0)
            {
                _groupExpenseRepository.RemoveRange(groupExpenseExist);
            }

            if (request.GroupExpenses != null)
            {
                request.GroupExpenses.ForEach(item =>
                {
                    item.MasterExpenseId = request.Id;
                    item.Id = Guid.NewGuid();
                });

                var groupExpense = _mapper.Map<List<GroupExpense>>(request.GroupExpenses);
                _groupExpenseRepository.AddRange(groupExpense);
            }

            if (await _uow.SaveAsync() <= 0)
            {

                _logger.LogError("Error while saving Master Expense");
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);

        }
    }
}
