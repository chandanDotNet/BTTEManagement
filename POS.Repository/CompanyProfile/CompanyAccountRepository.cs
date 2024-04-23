using AutoMapper;
using Microsoft.EntityFrameworkCore;
using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Data.Resources;
using POS.Data;
using POS.Domain;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BTTEM.Data.Resources;
using BTTEM.Data;
using BTTEM.Data.Dto;

namespace BTTEM.Repository
{
    public class CompanyAccountRepository : GenericRepository<CompanyAccount, POSDbContext>, ICompanyAccountRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;

        public CompanyAccountRepository(
            IUnitOfWork<POSDbContext> uow,
            IPropertyMappingService propertyMappingService,
             IMapper mapper)
            : base(uow)
        {
            _propertyMappingService = propertyMappingService;
        }

        public async Task<CompanyAccountList> GetCompanyAccounts(CompanyAccountResource companyAccountResource)
        {
            var collectionBeforePaging =
                AllIncluding().ApplySort(companyAccountResource.OrderBy,
                _propertyMappingService.GetPropertyMapping<CompanyAccountDto, CompanyAccount>());

            if (!string.IsNullOrEmpty(companyAccountResource.AccountName))
            {
                // trim & ignore casing
                var genreForWhereClause = companyAccountResource.AccountName
                    .Trim().ToLowerInvariant();
                var name = Uri.UnescapeDataString(genreForWhereClause);
                var encodingName = WebUtility.UrlDecode(name);
                var ecapestring = Regex.Unescape(encodingName);
                encodingName = encodingName.Replace(@"\", @"\\").Replace("%", @"\%").Replace("_", @"\_").Replace("[", @"\[").Replace(" ", "%");
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.AccountName, $"{encodingName}%"));
            }
            var CompanyAccountList = new CompanyAccountList();
            return await CompanyAccountList.Create(collectionBeforePaging, companyAccountResource.Skip, companyAccountResource.PageSize);
        }
    }
}