using AutoMapper;
using BTTEM.Data.Entities;
using BTTEM.MediatR.Commands;
using BTTEM.MediatR.Handler;
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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Handlers
{
    public class UpdateLocalConveyanceExpenseCommandHandler : IRequestHandler<UpdateLocalConveyanceExpenseCommand, ServiceResponse<bool>>
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly UserInfoToken _userInfoToken;
        private readonly IMasterExpenseRepository _masterExpenseRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateLocalConveyanceExpenseCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;
        private readonly ILocalConveyanceExpenseRepository _localConveyanceExpenseRepository;
        private readonly ILocalConveyanceExpenseDocumentRepository _localConveyanceExpenseDocumentRepository;

        public UpdateLocalConveyanceExpenseCommandHandler(
            IMasterExpenseRepository masterExpenseRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<UpdateLocalConveyanceExpenseCommandHandler> logger,
            IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper,
            UserInfoToken userInfoToken,
            IUserRoleRepository userRoleRepository,
            ILocalConveyanceExpenseRepository localConveyanceExpenseRepository,
            ILocalConveyanceExpenseDocumentRepository localConveyanceExpenseDocumentRepository)
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
        }

        public async Task<ServiceResponse<bool>> Handle(UpdateLocalConveyanceExpenseCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _localConveyanceExpenseRepository.FindAsync(request.Id);
            if (entityExist == null)
            {
                _logger.LogError("Expense does not exists.");
                return ServiceResponse<bool>.Return409("Expense does not exists.");
            }

            entityExist.ExpenseDate = request.ExpenseDate;

            if (!string.IsNullOrEmpty(request.Status))
            {
                entityExist.Status = request.Status;
            }
            if (!string.IsNullOrEmpty(request.Particular))
            {
                entityExist.Particular = request.Particular;
            }
            if (!string.IsNullOrEmpty(request.ModeOfTransport))
            {
                entityExist.ModeOfTransport = request.ModeOfTransport;
            }
            if (request.Amount > 0)
            {
                entityExist.Amount = request.Amount;
            }
            if (request.ApproxKM > 0)
            {
                entityExist.ApproxKM = request.ApproxKM;
            }
            if (request.GrandTotal > 0)
            {
                entityExist.GrandTotal = request.GrandTotal;
            }
            if (!string.IsNullOrEmpty(request.From))
            {
                entityExist.From = request.From;
            }
            if (!string.IsNullOrEmpty(request.To))
            {
                entityExist.To = request.To;
            }
            if (!string.IsNullOrEmpty(request.Place))
            {
                entityExist.Place = request.Place;
            }
            if (!string.IsNullOrEmpty(request.Remarks))
            {
                entityExist.Remarks = request.Remarks;
            }

            foreach (var item in request.Documents)
            {
                var entityExpenseDocument = _mapper.Map<LocalConveyanceExpenseDocument>(item);
                entityExpenseDocument.LocalConveyanceExpenseId = entityExist.Id;
                entityExpenseDocument.Id = Guid.NewGuid();

                if (!string.IsNullOrWhiteSpace(item.ReceiptName) && !string.IsNullOrWhiteSpace(item.ReceiptPath))
                {
                    string contentRootPath = _webHostEnvironment.WebRootPath;
                    var pathToSave = Path.Combine(contentRootPath, _pathHelper.Attachments);

                    if (!Directory.Exists(pathToSave))
                    {
                        Directory.CreateDirectory(pathToSave);
                    }

                    var extension = Path.GetExtension(item.ReceiptName);
                    var id = Guid.NewGuid();
                    var path = $"{id}.{extension}";
                    var documentPath = Path.Combine(pathToSave, path);
                    string base64 = item.ReceiptPath.Split(',').LastOrDefault();
                    if (!string.IsNullOrWhiteSpace(base64))
                    {
                        byte[] bytes = Convert.FromBase64String(base64);
                        try
                        {
                            await File.WriteAllBytesAsync($"{documentPath}", bytes);
                            entityExpenseDocument.ReceiptPath = path;
                        }
                        catch
                        {
                            _logger.LogError("Error while saving files", entityExpenseDocument);
                        }
                    }
                }
                _localConveyanceExpenseDocumentRepository.Add(entityExpenseDocument);
            }

            _localConveyanceExpenseRepository.Update(entityExist);


            if (await _uow.SaveAsync() <= 0)
            {

                _logger.LogError("Error while saving Master Expense");
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);

        }

    }
}
