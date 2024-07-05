using AutoMapper;
using BTTEM.Data.Dto;
using BTTEM.Data.Entities;
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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Handlers
{
    public class AddLocalConveyanceExpenseDocumentCommandHandler : IRequestHandler<AddLocalConveyanceExpenseDocumentCommand, ServiceResponse<LocalConveyanceExpenseDocumentDto>>
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly UserInfoToken _userInfoToken;
        private readonly IMasterExpenseRepository _masterExpenseRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddLocalConveyanceExpenseDocumentCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;
        private readonly ILocalConveyanceExpenseRepository _localConveyanceExpenseRepository;
        private readonly ICarBikeLogBookExpenseRepository _carBikeLogBookExpenseRepository;
        private readonly ICarBikeLogBookExpenseDocumentRepository _carBikeLogBookExpenseDocumentRepository;
        private readonly ILocalConveyanceExpenseDocumentRepository _localConveyanceExpenseDocumentRepository;

        public AddLocalConveyanceExpenseDocumentCommandHandler(
            IMasterExpenseRepository masterExpenseRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<AddLocalConveyanceExpenseDocumentCommandHandler> logger,
            IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper,
            UserInfoToken userInfoToken,
            IUserRoleRepository userRoleRepository,
            ILocalConveyanceExpenseRepository localConveyanceExpenseRepository,
            ICarBikeLogBookExpenseRepository carBikeLogBookExpenseRepository,
            ICarBikeLogBookExpenseDocumentRepository carBikeLogBookExpenseDocumentRepository,
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
            _carBikeLogBookExpenseRepository = carBikeLogBookExpenseRepository;
            _carBikeLogBookExpenseDocumentRepository = carBikeLogBookExpenseDocumentRepository;
            _localConveyanceExpenseDocumentRepository = localConveyanceExpenseDocumentRepository;
        }

        public async Task<ServiceResponse<LocalConveyanceExpenseDocumentDto>> Handle(AddLocalConveyanceExpenseDocumentCommand request, CancellationToken cancellationToken)
        {
            // Guid LoginUserId = Guid.Parse(_userInfoToken.Id);

            var entity = _mapper.Map<LocalConveyanceExpenseDocument>(request);
            entity.Id = Guid.NewGuid();


            if (!string.IsNullOrWhiteSpace(request.ReceiptName) && !string.IsNullOrWhiteSpace(request.ReceiptPath))
            {
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
                string base64 = request.ReceiptPath.Split(',').LastOrDefault();
                if (!string.IsNullOrWhiteSpace(base64))
                {
                    byte[] bytes = Convert.FromBase64String(base64);
                    try
                    {
                        await File.WriteAllBytesAsync($"{documentPath}", bytes);
                        entity.ReceiptPath = path;
                    }
                    catch
                    {
                        _logger.LogError("Error while saving files", entity);
                    }
                }
            }

            _localConveyanceExpenseDocumentRepository.Add(entity);

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving Master Expense");
                return ServiceResponse<LocalConveyanceExpenseDocumentDto>.Return500();
            }

            var industrydto = _mapper.Map<LocalConveyanceExpenseDocumentDto>(entity);
            return ServiceResponse<LocalConveyanceExpenseDocumentDto>.ReturnResultWith200(industrydto);


        }

    }
}
