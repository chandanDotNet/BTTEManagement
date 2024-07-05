using AutoMapper;
using BTTEM.Data.Entities;
using BTTEM.MediatR.Commands;
using BTTEM.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
    public class UpdateCarBikeLogBookExpenseCommandHandler : IRequestHandler<UpdateCarBikeLogBookExpenseCommand, ServiceResponse<bool>>
    {

        private readonly IUserRoleRepository _userRoleRepository;
        private readonly UserInfoToken _userInfoToken;
        private readonly IMasterExpenseRepository _masterExpenseRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCarBikeLogBookExpenseCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;
        private readonly ILocalConveyanceExpenseRepository _localConveyanceExpenseRepository;
        private readonly ICarBikeLogBookExpenseRepository _carBikeLogBookExpenseRepository;
        private readonly ICarBikeLogBookExpenseDocumentRepository _carBikeLogBookExpenseDocumentRepository;

        public UpdateCarBikeLogBookExpenseCommandHandler(
            IMasterExpenseRepository masterExpenseRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<UpdateCarBikeLogBookExpenseCommandHandler> logger,
            IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper,
            UserInfoToken userInfoToken,
            IUserRoleRepository userRoleRepository,
            ILocalConveyanceExpenseRepository localConveyanceExpenseRepository,
            ICarBikeLogBookExpenseRepository carBikeLogBookExpenseRepository,
            ICarBikeLogBookExpenseDocumentRepository carBikeLogBookExpenseDocumentRepository)
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
            _carBikeLogBookExpenseRepository = carBikeLogBookExpenseRepository;
            _carBikeLogBookExpenseDocumentRepository = carBikeLogBookExpenseDocumentRepository;
        }

        public async Task<ServiceResponse<bool>> Handle(UpdateCarBikeLogBookExpenseCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _carBikeLogBookExpenseRepository.FindAsync(request.Id);
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
            if (!string.IsNullOrEmpty(request.PlaceOfVisitDepartment))
            {
                entityExist.PlaceOfVisitDepartment = request.PlaceOfVisitDepartment;
            }

            if (request.StartingKMS > 0)
            {
                entityExist.StartingKMS = request.StartingKMS;
            }
            if (request.EndingKMS > 0)
            {
                entityExist.EndingKMS = request.EndingKMS;
            }
            if (request.ConsumptionKMS > 0)
            {
                entityExist.ConsumptionKMS = request.ConsumptionKMS;
            }
            if (request.RefillingLiters > 0)
            {
                entityExist.RefillingLiters = request.RefillingLiters;
            }
            if (request.RefillingAmount > 0)
            {
                entityExist.RefillingAmount = request.RefillingAmount;
            }
            if (!string.IsNullOrEmpty(request.From))
            {
                entityExist.From = request.From;
            }
            if (!string.IsNullOrEmpty(request.To))
            {
                entityExist.To = request.To;
            }
            if (!string.IsNullOrEmpty(request.FuelBillNo))
            {
                entityExist.FuelBillNo = request.FuelBillNo;
            }
            if (!string.IsNullOrEmpty(request.Remarks))
            {
                entityExist.Remarks = request.Remarks;
            }

            

            foreach (var item in request.Documents)
            {
                var entityExpenseDocument = _mapper.Map<CarBikeLogBookExpenseDocument>(item);
                entityExpenseDocument.CarBikeLogBookExpenseId = entityExist.Id;
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
                _carBikeLogBookExpenseDocumentRepository.Add(entityExpenseDocument);
            }
            _carBikeLogBookExpenseRepository.Update(entityExist);

            if (await _uow.SaveAsync() <= 0)
            {

                _logger.LogError("Error while saving Master Expense");
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);

        }
    }
}
