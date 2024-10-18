using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Handlers;
using BTTEM.MediatR.PoliciesTravel.Commands;
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

namespace BTTEM.MediatR.PoliciesTravel.Handlers
{
    public class UpdatePoliciesDetailCommandHandler : IRequestHandler<UpdatePoliciesDetailCommand, ServiceResponse<bool>>
    {

        private readonly IPoliciesDetailRepository _policiesDetailRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly UserInfoToken _userInfoToken;
        private readonly ILogger<UpdatePoliciesDetailCommandHandler> _logger;
        private readonly PathHelper _pathHelper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UpdatePoliciesDetailCommandHandler(
           IPoliciesDetailRepository policiesDetailRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            UserInfoToken userInfoToken,
            ILogger<UpdatePoliciesDetailCommandHandler> logger,
            PathHelper pathHelper,
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

        public async Task<ServiceResponse<bool>> Handle(UpdatePoliciesDetailCommand request, CancellationToken cancellationToken)
        {
            var policiesDetailUpdate = _mapper.Map<Data.PoliciesDetail>(request);

            var policiesDetailExit = await _policiesDetailRepository.FindAsync(request.Id);

            policiesDetailExit.Name = policiesDetailUpdate.Name;
            policiesDetailExit.Description = policiesDetailUpdate.Description;
            policiesDetailExit.GradeId = policiesDetailUpdate.GradeId;
            
            policiesDetailExit.DailyAllowance = policiesDetailUpdate.DailyAllowance;
            policiesDetailExit.IsActive= policiesDetailUpdate.IsActive;
            policiesDetailExit.CompanyAccountId= policiesDetailUpdate.CompanyAccountId;

            //if (!string.IsNullOrEmpty(request.PolicyDocument))
            //{
            //    if (!string.IsNullOrEmpty(request.PolicyDocument))
            //    {
            //        policiesDetailExit.Document = $"{Guid.NewGuid()}.png";
            //    }               
            //}

            if (!string.IsNullOrEmpty(request.Document))
            {
                if (!string.IsNullOrWhiteSpace(request.Document) && !string.IsNullOrWhiteSpace(request.PolicyDocument))
                {
                    string contentRootPath = _webHostEnvironment.WebRootPath;

                    if (!string.IsNullOrWhiteSpace(policiesDetailExit.Document)
                    && File.Exists(Path.Combine(contentRootPath, _pathHelper.PolicyDocumentPath, policiesDetailExit.Document)))
                    {
                        FileData.DeleteFile(Path.Combine(contentRootPath, _pathHelper.PolicyDocumentPath, policiesDetailExit.Document));
                    }
                    if (!string.IsNullOrEmpty(policiesDetailUpdate.Document))
                    {
                        policiesDetailExit.Document = policiesDetailUpdate.Document;
                    }
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
                            policiesDetailExit.Document = path;
                            await FileData.SaveFile(Path.Combine(pathToSave, policiesDetailExit.Document), request.PolicyDocument);
                        }
                        catch
                        {
                            _logger.LogError("Error while saving files", policiesDetailExit);
                        }
                    }
                }
                else
                {
                    policiesDetailExit.Document = null;
                }
            }

            _policiesDetailRepository.Update(policiesDetailExit);

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while Updating Lodging Fooding Mode.");
                return ServiceResponse<bool>.Return500();
            }

            //if (!string.IsNullOrEmpty(request.PolicyDocument))
            //{
            //    string contentRootPath = _webHostEnvironment.WebRootPath;
            //    // delete old file
            //    if (!string.IsNullOrWhiteSpace(policiesDetailExit.Document)
            //        && File.Exists(Path.Combine(contentRootPath, _pathHelper.PolicyDocumentPath, policiesDetailExit.Document)))
            //    {
            //        FileData.DeleteFile(Path.Combine(contentRootPath, _pathHelper.PolicyDocumentPath, policiesDetailExit.Document));
            //    }

            //    // save new file
            //    if (!string.IsNullOrWhiteSpace(request.PolicyDocument))
            //    {
            //        var pathToSave = Path.Combine(contentRootPath, _pathHelper.PolicyDocumentPath);
            //        if (!Directory.Exists(pathToSave))
            //        {
            //            Directory.CreateDirectory(pathToSave);
            //        }
            //        await FileData.SaveFile(Path.Combine(pathToSave, policiesDetailExit.Document), request.PolicyDocument);
            //    }
            //}

            return ServiceResponse<bool>.ReturnResultWith201(true);
        }
    }
}
