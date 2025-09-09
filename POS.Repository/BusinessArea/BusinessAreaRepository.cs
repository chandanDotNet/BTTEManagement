using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.Data.Resources;
using Microsoft.EntityFrameworkCore;
using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data.Resources;
using POS.Domain;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BTTEM.Repository
{
    public class BusinessAreaRepository : GenericRepository<BusinessArea, POSDbContext>, IBusinessAreaRepository
    {
        public IPropertyMappingService _propertyMappingService { get; set; }
        public BusinessAreaRepository(
            IUnitOfWork<POSDbContext> uow,
            IPropertyMappingService propertyMappingService,
            IMapper _mapper) : base(uow)
        {
            _propertyMappingService = propertyMappingService;
        }

        public async Task<BusinessAreaList> GetBusinessAreas(BusinessAreaResource businessAreaResource)
        {
            var collectionBeforePaging =
               AllIncluding().ApplySort(businessAreaResource.OrderBy,
               _propertyMappingService.GetPropertyMapping<BusinessAreaDto, BusinessArea>());

            if (!string.IsNullOrEmpty(businessAreaResource.SearchQuery))
            {
                // trim & ignore casing
                var genreForWhereClause = businessAreaResource.SearchQuery
                    .Trim().ToLowerInvariant();
                var name = Uri.UnescapeDataString(genreForWhereClause);
                var encodingName = WebUtility.UrlDecode(name);
                var ecapestring = Regex.Unescape(encodingName);
                encodingName = encodingName.Replace(@"\", @"\\").Replace("%", @"\%").Replace("_", @"\_").Replace("[", @"\[").Replace(" ", "%");
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.CostCenterBranchName, $"{encodingName}%")
                    || EF.Functions.Like(a.BusinessAreaName, $"{encodingName}%")
                    || EF.Functions.Like(a.BusinessPlace, $"{encodingName}%")
                    || EF.Functions.Like(a.ProfitCenter, $"{encodingName}%")
                    || EF.Functions.Like(a.BusinessAreaStateName, $"{encodingName}%"));
            }
            if (businessAreaResource.CompanyAccountId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.CompanyAccountId == businessAreaResource.CompanyAccountId);
            }
            var BusinessAreaList = new BusinessAreaList();
            return await BusinessAreaList.Create(collectionBeforePaging, businessAreaResource.Skip, businessAreaResource.PageSize);
        }
    }
}