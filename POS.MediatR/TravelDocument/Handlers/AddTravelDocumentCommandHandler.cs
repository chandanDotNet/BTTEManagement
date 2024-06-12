using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
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
    public class AddTravelDocumentCommandHandler : IRequestHandler<AddTravelDocumentCommand, ServiceResponse<TravelDocumentDto>>
    {

        private readonly ITravelDocumentRepository _travelDocumentRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddTravelDocumentCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;

        public AddTravelDocumentCommandHandler(
            ITravelDocumentRepository travelDocumentRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<AddTravelDocumentCommandHandler> logger,
            IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper)
        {
            _travelDocumentRepository = travelDocumentRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
        }


        public async Task<ServiceResponse<TravelDocumentDto>> Handle(AddTravelDocumentCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Data.TravelDocument>(request);

            if (!string.IsNullOrWhiteSpace(request.FileName) && !string.IsNullOrWhiteSpace(request.DocumentData))
            {
                string contentRootPath = _webHostEnvironment.WebRootPath;
                var pathToSave = Path.Combine(contentRootPath, _pathHelper.TravelDocument);

                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }

                var extension = Path.GetExtension(request.FileName);
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
                        entity.ReceiptPath = path;
                    }
                    catch
                    {
                        _logger.LogError("Error while saving files", entity);
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(request.FileNameBack) && !string.IsNullOrWhiteSpace(request.DocumentDataBack))
            {
                string contentRootPath = _webHostEnvironment.WebRootPath;
                var pathToSave = Path.Combine(contentRootPath, _pathHelper.TravelDocument);

                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }

                var extension = Path.GetExtension(request.FileNameBack);
                var id = Guid.NewGuid();
                var path = $"{id}.{extension}";
                var documentPath = Path.Combine(pathToSave, path);
                string base64 = request.DocumentDataBack.Split(',').LastOrDefault();
                if (!string.IsNullOrWhiteSpace(base64))
                {
                    byte[] bytes = Convert.FromBase64String(base64);
                    try
                    {
                        await File.WriteAllBytesAsync($"{documentPath}", bytes);
                        entity.ReceiptPathBack = path;
                    }
                    catch
                    {
                        _logger.LogError("Error while saving files", entity);
                    }
                }
            }

            _travelDocumentRepository.Add(entity);

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving Expense");
                return ServiceResponse<TravelDocumentDto>.Return500();
            }

            var industrydto = _mapper.Map<TravelDocumentDto>(entity);
            return ServiceResponse<TravelDocumentDto>.ReturnResultWith200(industrydto);
        }

    }
}
