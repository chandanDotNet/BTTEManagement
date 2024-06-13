using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Handlers;
using BTTEM.MediatR.TravelDocument.Commands;
using BTTEM.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.TravelDocument.Handlers
{
    public class UpdateTravelDocumentCommandHandler : IRequestHandler<UpdateTravelDocumentCommand, ServiceResponse<bool>>
    {

        private readonly ITravelDocumentRepository _travelDocumentRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateTravelDocumentCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;

        public UpdateTravelDocumentCommandHandler(
            ITravelDocumentRepository travelDocumentRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<UpdateTravelDocumentCommandHandler> logger,
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

        public async Task<ServiceResponse<bool>> Handle(UpdateTravelDocumentCommand request, CancellationToken cancellationToken)
        {
           // var entity = _mapper.Map<Data.TravelDocument>(request);
            var entity = await _travelDocumentRepository.FindBy(v => v.Id == request.Id).FirstOrDefaultAsync();

            if(request.IsChangeFileFont==true)
            {
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
                }else
                {
                    entity.ReceiptPath = null;
                    entity.FileName = null;
                }
            }

            if (request.IsChangeFileBack == true)
            {
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
                else
                {
                    entity.ReceiptPathBack = null;
                    entity.FileNameBack = null;

                }
            }

               

            entity.DocNumber = request.DocNumber;
            entity.IssuedOn = request.IssuedOn;
            entity.DocType = request.DocType;
            entity.IsVerified = request.IsVerified;
            entity.ValidTill = request.ValidTill;

            //_travelDocumentRepository.Add(entity);

            _travelDocumentRepository.Update(entity);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<bool>.Return500();
            }

            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
