using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data;
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
    public class AddPoliciesDetailCommandHandler : IRequestHandler<AddPoliciesDetailCommand, ServiceResponse<PoliciesDetailDto>>
    {

        private readonly IPoliciesDetailRepository _policiesDetailRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly UserInfoToken _userInfoToken;
        private readonly ILogger<AddPoliciesDetailCommandHandler> _logger;
        private readonly POS.Helper.PathHelper _pathHelper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AddPoliciesDetailCommandHandler(
           IPoliciesDetailRepository policiesDetailRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            UserInfoToken userInfoToken,
            ILogger<AddPoliciesDetailCommandHandler> logger,
            POS.Helper.PathHelper pathHelper,
            IWebHostEnvironment webHostEnvironment
            )
        {
            _policiesDetailRepository = policiesDetailRepository;
            _mapper = mapper;
            _uow = uow;
            _userInfoToken = userInfoToken;
            _logger = logger;
            _pathHelper = pathHelper;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<ServiceResponse<PoliciesDetailDto>> Handle(AddPoliciesDetailCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _policiesDetailRepository.FindBy(c => c.Name == request.Name).FirstOrDefaultAsync();
            if (entityExist != null)
            {
                _logger.LogError("Policies Name already exist.");
                return ServiceResponse<PoliciesDetailDto>.Return409("Policies Name already exist.");
            }

            var entity = _mapper.Map<Data.PoliciesDetail>(request);
            entity.Id = Guid.NewGuid();
            entity.CreatedBy = Guid.Parse(_userInfoToken.Id);
            //entity.CreatedDate = DateTime.UtcNow;
            entity.CreatedDate = DateTime.Now;
            entity.ModifiedBy = Guid.Parse(_userInfoToken.Id);


            if (!string.IsNullOrWhiteSpace(request.Document) && !string.IsNullOrWhiteSpace(request.Document))
            {
                string contentRootPath = _webHostEnvironment.WebRootPath;
                var pathToSave = Path.Combine(contentRootPath, _pathHelper.PolicyDocumentPath);

                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }

                var extension = Path.GetExtension(request.Document);
                var id = Guid.NewGuid();
                var path = $"{id}.{extension}";
                var documentPath = Path.Combine(pathToSave, path);
                string base64 = request.PolicyDocument.Split(',').LastOrDefault();
                if (!string.IsNullOrWhiteSpace(base64))
                {
                    byte[] bytes = Convert.FromBase64String(base64);
                    try
                    {
                        await File.WriteAllBytesAsync($"{documentPath}", bytes);
                        entity.Document = path;
                        await FileData.SaveFile(Path.Combine(pathToSave, entity.Document), request.PolicyDocument);
                    }
                    catch
                    {
                        _logger.LogError("Error while saving files", entity);
                    }
                }
            }

            //if (!string.IsNullOrWhiteSpace(request.PolicyDocument))
            //{
            //    entity.Document = Guid.NewGuid().ToString() + ".png";
            //}

            _policiesDetailRepository.Add(entity);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<PoliciesDetailDto>.Return500();
            }

            //if (!string.IsNullOrWhiteSpace(request.PolicyDocument))
            //{
            //    var pathToSave = Path.Combine(_webHostEnvironment.WebRootPath, _pathHelper.PolicyDocumentPath);
            //    if (!Directory.Exists(pathToSave))
            //    {
            //        Directory.CreateDirectory(pathToSave);
            //    }
            //    await FileData.SaveFile(Path.Combine(pathToSave, entity.Document), request.PolicyDocument);
            //}


            var entityDto = _mapper.Map<PoliciesDetailDto>(entity);
            if (!string.IsNullOrWhiteSpace(request.Document))
            {
                entityDto.Document = Path.Combine(_pathHelper.PolicyDocumentPath, entityDto.Document);
            }

            return ServiceResponse<PoliciesDetailDto>.ReturnResultWith200(entityDto);
        }
    }
}
