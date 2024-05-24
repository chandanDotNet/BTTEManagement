using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.RequestACall.Handler
{
    public class AddRequestCallCommandHandler : IRequestHandler<AddRequestCallCommand, ServiceResponse<RequestCallDto>>
    {
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly IRequestCallRepository _requestCallRepository;
        private readonly ILogger<AddRequestCallCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;
        public AddRequestCallCommandHandler(IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            IRequestCallRepository requestCallRepository,
            ILogger<AddRequestCallCommandHandler> logger,
            IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper)
        {
            _mapper = mapper;
            _uow = uow;
            _requestCallRepository = requestCallRepository;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
        }

        public async Task<ServiceResponse<RequestCallDto>> Handle(AddRequestCallCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<RequestCall>(request);
            entity.Id = Guid.NewGuid();

            Random rnd = new Random();
            entity.RequestNo = "Req-" + Convert.ToString(rnd.Next(999999));

            if (!string.IsNullOrWhiteSpace(request.DocumentName) && !string.IsNullOrEmpty(request.DocumentData))
            {
                string contentRootPath = _webHostEnvironment.WebRootPath;
                string pathToSave = Path.Combine(contentRootPath, _pathHelper.RequestCallDocument);
                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }
                var extension = Path.GetExtension(request.DocumentName);
                var id = Guid.NewGuid();
                var path = $"{id}{extension}";
                var documentPath = Path.Combine(pathToSave, path);

                var base64 = request.DocumentData.Split(',').LastOrDefault();
                if (!string.IsNullOrWhiteSpace(base64))
                {
                    byte[] bytes = Convert.FromBase64String(base64);
                    try
                    {
                        await File.WriteAllBytesAsync($"{documentPath}", bytes);
                        entity.DocumentName = path;
                    }
                    catch (Exception)
                    {
                        _logger.LogError("Error while saving files", entity);
                    }
                }
            }
            _requestCallRepository.Add(entity);

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving request call");
                return ServiceResponse<RequestCallDto>.Return500();
            }

            var requestCallDto = _mapper.Map<RequestCallDto>(entity);
            return ServiceResponse<RequestCallDto>.ReturnResultWith200(requestCallDto);

        }
    }
}
