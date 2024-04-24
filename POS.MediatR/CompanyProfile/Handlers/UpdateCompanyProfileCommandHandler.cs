using AutoMapper;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BTTEM.Repository;
using Microsoft.EntityFrameworkCore;
using BTTEM.Data;

namespace POS.MediatR.Handlers
{
    public class UpdateCompanyProfileCommandHandler
        : IRequestHandler<UpdateCompanyProfileCommand, ServiceResponse<CompanyProfileDto>>
    {
        private readonly ICompanyProfileRepository _companyProfileRepository;
        private readonly ICompanyAccountRepository _companyAccountRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<UpdateCompanyProfileCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;

        public UpdateCompanyProfileCommandHandler(
            ICompanyProfileRepository companyProfileRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<UpdateCompanyProfileCommandHandler> logger,
            IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper,
            ICompanyAccountRepository companyAccountRepository)
        {
            _companyProfileRepository = companyProfileRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
            _companyAccountRepository = companyAccountRepository;
        }
        public async Task<ServiceResponse<CompanyProfileDto>> Handle(UpdateCompanyProfileCommand request, CancellationToken cancellationToken)
        {
            var logoUrl = string.Empty;

            if (!string.IsNullOrWhiteSpace(request.ImageData))
            {
                logoUrl = $"{Guid.NewGuid()}.{Path.GetExtension(request.LogoUrl)}";
            }

            CompanyProfile companyProfile;
            if (request.Id.HasValue)
            {
                companyProfile = await _companyProfileRepository.FindAsync(request.Id.Value);
                if (companyProfile != null)
                {
                    companyProfile.Title = request.Title;
                    companyProfile.Address = request.Address;
                    companyProfile.Phone = request.Phone;
                    companyProfile.Email = request.Email;
                    companyProfile.CurrencyCode = request.CurrencyCode;
                    companyProfile.CountryName = request.CountryName;
                    companyProfile.CityName = request.CityName;
                    companyProfile.CountryId = request.CountryId;
                    companyProfile.CityId = request.CityId;
                    companyProfile.ZipCode = request.ZipCode;
                    companyProfile.Fax = request.Fax;
                    companyProfile.WebSite = request.WebSite;
                    companyProfile.FY = request.FY;
                    companyProfile.TimeZone = request.TimeZone;

                    if (!string.IsNullOrWhiteSpace(request.ImageData))
                    {
                        companyProfile.LogoUrl = logoUrl;
                    }
                    _companyProfileRepository.Update(companyProfile);

                    var updateCompanyAccounts = await _companyAccountRepository.All.Where(x => x.CompanyProfileId == request.CompanyAccounts.FirstOrDefault().CompanyProfileId).ToListAsync();
                    if (updateCompanyAccounts.Count > 0)
                    {
                        for (int i = 0; i < updateCompanyAccounts.Count; i++)
                        {
                            updateCompanyAccounts[i].AccountName = request.CompanyAccounts[i].AccountName;
                        }
                        _companyAccountRepository.UpdateRange(updateCompanyAccounts);
                    }

                    var addCompanyAccounts = request.CompanyAccounts.Where(x => x.Id == Guid.Empty).ToList();
                    if (addCompanyAccounts.Count > 0)
                    {
                        var newCompanyAccounts = _mapper.Map<List<CompanyAccount>>(addCompanyAccounts);
                        for (int i = 0; i < newCompanyAccounts.Count; i++)
                        {
                            newCompanyAccounts[i].Id = Guid.NewGuid();
                            newCompanyAccounts[i].AccountName = addCompanyAccounts[i].AccountName;
                            newCompanyAccounts[i].CompanyProfileId = request.Id.Value;
                        }
                        _companyAccountRepository.AddRange(newCompanyAccounts);
                    }
                }
                else
                {
                    companyProfile = new CompanyProfile
                    {
                        Address = request.Address,
                        Title = request.Title
                    };
                    if (!string.IsNullOrWhiteSpace(request.ImageData))
                    {
                        companyProfile.LogoUrl = logoUrl;
                    }
                    _companyProfileRepository.Add(companyProfile);
                }
            }
            else
            {
                companyProfile = _mapper.Map<CompanyProfile>(request);
                if (!string.IsNullOrWhiteSpace(request.ImageData))
                {
                    companyProfile.LogoUrl = logoUrl;
                }
                _companyProfileRepository.Add(companyProfile);
            }

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while Updating Company Profile.");
                return ServiceResponse<CompanyProfileDto>.Return500();
            }

            if (!string.IsNullOrWhiteSpace(request.ImageData))
            {
                string pathToSave = _webHostEnvironment.WebRootPath;
                pathToSave = Path.Combine(pathToSave, _pathHelper.CompanyLogo);
                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }
                var documentPath = Path.Combine(pathToSave, companyProfile.LogoUrl);
                string base64 = request.ImageData.Split(',').LastOrDefault();
                if (!string.IsNullOrWhiteSpace(base64))
                {
                    byte[] bytes = Convert.FromBase64String(base64);
                    try
                    {
                        await File.WriteAllBytesAsync($"{documentPath}", bytes);
                    }
                    catch
                    {
                        _logger.LogError("Error while saving files");
                    }
                }
            }
            var result = _mapper.Map<CompanyProfileDto>(companyProfile);
            if (!string.IsNullOrWhiteSpace(result.LogoUrl))
            {
                result.LogoUrl = Path.Combine(_pathHelper.CompanyLogo, result.LogoUrl);
            }
            return ServiceResponse<CompanyProfileDto>.ReturnResultWith200(result);
        }
    }
}
