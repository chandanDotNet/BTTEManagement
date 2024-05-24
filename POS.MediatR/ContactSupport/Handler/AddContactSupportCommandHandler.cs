using AutoMapper;
using BTTEM.Data.Dto;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.RequestACall.Handler;
using BTTEM.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
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

namespace BTTEM.MediatR.ContactSupport.Handler
{
    public class AddContactSupportCommandHandler : IRequestHandler<AddContactSupportCommand, ServiceResponse<ContactSupportDto>>
    {
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly IContactSupportRepository _contactSupportRepository;
        private readonly ILogger<AddRequestCallCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;
        public AddContactSupportCommandHandler(IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            IContactSupportRepository contactSupportRepository,
            ILogger<AddRequestCallCommandHandler> logger,
            IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper)
        {
            _uow = uow;
            _mapper = mapper;
            _contactSupportRepository = contactSupportRepository;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
        }

        public async Task<ServiceResponse<ContactSupportDto>> Handle(AddContactSupportCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Data.ContactSupport>(request);
            entity.Id = Guid.NewGuid();

            Random rnd = new Random();
            entity.RequestNo = "Req-" + Convert.ToString(rnd.Next(999999));

            if (!string.IsNullOrEmpty(request.DocumentData) && !string.IsNullOrEmpty(request.DocumentData))
            {
                var contentRootPath = _webHostEnvironment.WebRootPath;
                string pathToSave = Path.Combine(contentRootPath, _pathHelper.ContactSupportDocument);
                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }

                var extension = Path.GetExtension(request.DocumentName);
                var id = Guid.NewGuid();
                var path = $"{id}{extension}";
                var documentPath = Path.Combine(pathToSave, path);

                var base64 = request.DocumentData.Split(',').LastOrDefault();
                if (!string.IsNullOrEmpty(base64))
                {
                    byte[] bytes = Convert.FromBase64String(base64);
                    try
                    {
                        await File.WriteAllBytesAsync($"{documentPath}", bytes);
                        entity.DocumentName= path;
                    }
                    catch (Exception)
                    {
                        _logger.LogError("Error while saving files", entity);
                    }
                }
            }
            _contactSupportRepository.Add(entity);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving contact support");
                return ServiceResponse<ContactSupportDto>.Return500();
            }

            var contactSupportDto = _mapper.Map<ContactSupportDto>(entity);
            return ServiceResponse<ContactSupportDto>.ReturnResultWith200(contactSupportDto);
        }
    }
}
