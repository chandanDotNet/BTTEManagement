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
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.MediatR.Handlers;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly UserInfoToken _userInfoToken;
        private ICompanyAccountRepository _companyAccountRepository;
        public UpdateMasterExpenseCommandHandler(
            IMasterExpenseRepository masterExpenseRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<UpdateMasterExpenseCommandHandler> logger,
            IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper,
            IGroupExpenseRepository groupExpenseRepository,
            UserInfoToken userInfoToken,
            ICompanyAccountRepository companyAccountRepository)
        {
            _masterExpenseRepository = masterExpenseRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
            _groupExpenseRepository = groupExpenseRepository;
            _userInfoToken = userInfoToken;
            _companyAccountRepository = companyAccountRepository;
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

            //entityExist.CreatedDate = DateTime.Now;

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
            entityExist.PayableAmount = 0;
            entityExist.IsGroupExpense = request.IsGroupExpense;
            entityExist.NoOfPerson = request.NoOfPerson;

            entityExist.CompanyAccountId = request.CompanyAccountId;

            if (request.CompanyAccountId == new Guid("d0ccea5f-5393-4a34-9df6-43a9f51f9f91"))
            {               
                entityExist.AccountsCheckerOneStatus = string.IsNullOrEmpty(request.AccountsCheckerOneStatus) ? entityExist.AccountsCheckerOneStatus : "PENDING";
                entityExist.AccountsCheckerTwoStatus = string.IsNullOrEmpty(request.AccountsCheckerTwoStatus) ? entityExist.AccountsCheckerTwoStatus : "PENDING";
                entityExist.AccountsCheckerThreeStatus = string.IsNullOrEmpty(request.AccountsCheckerThreeStatus) ? entityExist.AccountsCheckerThreeStatus : "PENDING";
                entityExist.IsExpenseChecker = true;
                entityExist.AccountsApprovalStage = request.AccountsApprovalStage == null ? entityExist.AccountsApprovalStage : 0;
            }

            if (!string.IsNullOrWhiteSpace(request.DocumentData))
            {

                if (!string.IsNullOrWhiteSpace(request.ReceiptName))
                {
                    entityExist.ReceiptName = request.ReceiptName;
                }
                
                string contentRootPath = _webHostEnvironment.WebRootPath;
                var pathToSave = Path.Combine(contentRootPath, _pathHelper.Attachments);

                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }

                var extension = Path.GetExtension(request.ReceiptName);
                var id = Guid.NewGuid();
                var path = $"{id}.{extension}";
                var documentPath = Path.Combine(pathToSave, path);
                string base64 = request.DocumentData.Split(',').LastOrDefault();
                if (!string.IsNullOrWhiteSpace(base64))
                {
                    byte[] bytes = Convert.FromBase64String(base64);
                    try
                    {
                        await File.WriteAllBytesAsync($"{documentPath}", bytes);
                        entityExist.ReceiptPath = path;
                    }
                    catch
                    {
                        _logger.LogError("Error while saving files", entityExist);
                    }
                }
            }

            if (request.CompanyAccountId.HasValue && _userInfoToken.CompanyAccountId.HasValue)
            {
                request.AccountTeam = _userInfoToken.AccountTeam;

                //if (request.CompanyAccountId == _userInfoToken.CompanyAccountId)
                //{
                //    request.AccountTeam = _userInfoToken.AccountTeam;
                //}
                //else
                //{
                //    var company = _companyAccountRepository.All.Where(x => x.Id == request.CompanyAccountId).FirstOrDefault();
                //    request.AccountTeam = company.AccountTeam;
                //}
            }

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
