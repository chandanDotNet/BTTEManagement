using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.Data.Entities;
using BTTEM.MediatR.CommandAndQuery;
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
    public class AddCarBikeLogBookExpenseCommandHandler : IRequestHandler<AddCarBikeLogBookExpenseCommand, ServiceResponse<CarBikeLogBookExpenseDto>>
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly UserInfoToken _userInfoToken;
        private readonly IMasterExpenseRepository _masterExpenseRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddCarBikeLogBookExpenseCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;
        private readonly ILocalConveyanceExpenseRepository _localConveyanceExpenseRepository;
        private readonly ICarBikeLogBookExpenseRepository _carBikeLogBookExpenseRepository;

        public AddCarBikeLogBookExpenseCommandHandler(
            IMasterExpenseRepository masterExpenseRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<AddCarBikeLogBookExpenseCommandHandler> logger,
            IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper,
            UserInfoToken userInfoToken,
            IUserRoleRepository userRoleRepository,
            ILocalConveyanceExpenseRepository localConveyanceExpenseRepository,
            ICarBikeLogBookExpenseRepository carBikeLogBookExpenseRepository)
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
        }

        public async Task<ServiceResponse<CarBikeLogBookExpenseDto>> Handle(AddCarBikeLogBookExpenseCommand request, CancellationToken cancellationToken)
        {
            // Guid LoginUserId = Guid.Parse(_userInfoToken.Id);

            var entity = _mapper.Map<CarBikeLogBookExpense>(request);
            entity.Id = Guid.NewGuid();

            if (!string.IsNullOrWhiteSpace(request.RefillingDocument))
            {
                entity.RefillingUrl = Guid.NewGuid().ToString() + ".png";
            }

            if (!string.IsNullOrWhiteSpace(request.TollParkingDocument))
            {
                entity.TollParkingUrl = Guid.NewGuid().ToString() + ".png";
            }           

            _carBikeLogBookExpenseRepository.Add(entity);

            if (!string.IsNullOrWhiteSpace(request.RefillingDocument))
            {
                var pathToSave = Path.Combine(_webHostEnvironment.WebRootPath, _pathHelper.RefillingDocumnentPath);
                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }
                await FileData.SaveFile(Path.Combine(pathToSave, entity.RefillingUrl), request.RefillingDocument);
            }

            if (!string.IsNullOrWhiteSpace(request.TollParkingDocument))
            {
                var pathToSave = Path.Combine(_webHostEnvironment.WebRootPath, _pathHelper.TollParkingDocumnentPath);
                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }
                await FileData.SaveFile(Path.Combine(pathToSave, entity.RefillingUrl), request.TollParkingDocument);
            }

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving Master Expense");
                return ServiceResponse<CarBikeLogBookExpenseDto>.Return500();
            }

            int index = 0;
            foreach (var item in entity.Documents)
            {
                var entityExpenseDocument = _mapper.Map<CarBikeLogBookExpenseDocument>(item);
                entityExpenseDocument.CarBikeLogBookExpenseId = entity.Id;
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
                            entity.Documents[index].ReceiptPath = path;
                        }
                        catch
                        {
                            _logger.LogError("Error while saving files", entity);
                        }
                    }
                }
                index++;
            }

            var industrydto = _mapper.Map<CarBikeLogBookExpenseDto>(entity);

            if (!string.IsNullOrWhiteSpace(request.RefillingDocument))
            {
                industrydto.RefillingUrl = Path.Combine(_pathHelper.BrandImagePath, industrydto.RefillingUrl);
            }

            if (!string.IsNullOrWhiteSpace(request.TollParkingDocument))
            {
                industrydto.TollParkingUrl = Path.Combine(_pathHelper.BrandImagePath, industrydto.TollParkingUrl);
            }
            return ServiceResponse<CarBikeLogBookExpenseDto>.ReturnResultWith200(industrydto);


        }

    }
}
