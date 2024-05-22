using AutoMapper;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using POS.Data;
using BTTEM.Repository;

namespace POS.MediatR.Handlers
{
    public class UpdateExpenseCommandHandler
        : IRequestHandler<UpdateExpenseCommand, ServiceResponse<bool>>
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateExpenseCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;
        private readonly IMasterExpenseRepository _masterExpenseRepository;

        public UpdateExpenseCommandHandler(
            IExpenseRepository expenseRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<UpdateExpenseCommandHandler> logger,
            IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper,
            IMasterExpenseRepository masterExpenseRepository)
        {
            _expenseRepository = expenseRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
            _masterExpenseRepository = masterExpenseRepository;
        }

        public async Task<ServiceResponse<bool>> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
        {
            
            var entityExist = await _expenseRepository.FindAsync(request.Id);         

            if (request.MasterExpenseId == Guid.Empty || request.MasterExpenseId == null)
            {
                request.MasterExpenseId = entityExist.MasterExpenseId;
            }  
            
            if (entityExist == null)
            {
                _mapper.Map(request, entityExist);
                entityExist = _mapper.Map<Expense>(request);               
                _expenseRepository.Add(entityExist);
            }           
            else
            {
                _mapper.Map(request, entityExist);
                _expenseRepository.Update(entityExist);
            }

            if (request.IsReceiptChange)
            {
                if (!string.IsNullOrWhiteSpace(request.DocumentData)
                    && !string.IsNullOrWhiteSpace(request.ReceiptName))
                {
                    string contentRootPath = _webHostEnvironment.WebRootPath;
                    var pathToSave = Path.Combine(contentRootPath, _pathHelper.Attachments);

                    if (!Directory.Exists(pathToSave))
                    {
                        Directory.CreateDirectory(pathToSave);
                    }

                    var extension = Path.GetExtension(request.ReceiptName); ;
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
                else
                {
                    entityExist.ReceiptPath = null;
                    entityExist.ReceiptName = null;
                }
            }

            var entityMasterExist = await _masterExpenseRepository.FindAsync(entityExist.MasterExpenseId);
            if (entityMasterExist != null)
            {
                decimal UpdatedExpenseAmount = 0;
                decimal OldExpenseAmount = entityExist.Amount;
                decimal NowExpenseAmount = request.Amount;
                decimal TotalExpenseAmount = entityMasterExist.TotalAmount;
                UpdatedExpenseAmount = (TotalExpenseAmount - OldExpenseAmount);
                UpdatedExpenseAmount = UpdatedExpenseAmount + NowExpenseAmount;
                entityMasterExist.TotalAmount= UpdatedExpenseAmount;
                _masterExpenseRepository.Update(entityMasterExist);

            }
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving Expense.");
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnSuccess();
        }
    }
}
