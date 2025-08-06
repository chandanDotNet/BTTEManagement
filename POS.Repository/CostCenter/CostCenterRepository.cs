using AutoMapper;
using BTTEM.Data.Dto;
using BTTEM.Data.Resources;
using BTTEM.Data;
using Microsoft.EntityFrameworkCore;
using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
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
    public class CostCenterRepository : GenericRepository<CostCenter, POSDbContext>, ICostCenterRepository
    {
        public IPropertyMappingService _propertyMappingService { get; set; }
        public CostCenterRepository(
            IUnitOfWork<POSDbContext> uow,
            IPropertyMappingService propertyMappingService,
            IMapper _mapper) : base(uow)
        {
            _propertyMappingService = propertyMappingService;
        }

        public async Task<CostCenterList> GetCostCenters(CostCenterResource costCenterResource)
        {
            var collectionBeforePaging =
               AllIncluding().ApplySort(costCenterResource.OrderBy,
               _propertyMappingService.GetPropertyMapping<CostCenterDto, CostCenter>());

            if (!string.IsNullOrEmpty(costCenterResource.SearchQuery))
            {
                // trim & ignore casing
                var genreForWhereClause = costCenterResource.SearchQuery
                    .Trim().ToLowerInvariant();
                var name = Uri.UnescapeDataString(genreForWhereClause);
                var encodingName = WebUtility.UrlDecode(name);
                var ecapestring = Regex.Unescape(encodingName);
                encodingName = encodingName.Replace(@"\", @"\\").Replace("%", @"\%").Replace("_", @"\_").Replace("[", @"\[").Replace(" ", "%");
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.CostCenterBranchName, $"{encodingName}%")
                    || EF.Functions.Like(a.BusinessArea, $"{encodingName}%")
                    || EF.Functions.Like(a.BusinessPlace, $"{encodingName}%")
                    || EF.Functions.Like(a.ProfitCenter, $"{encodingName}%"));
            }

            var CostCenterList = new CostCenterList();
            return await CostCenterList.Create(collectionBeforePaging, costCenterResource.Skip, costCenterResource.PageSize);
        }
    }
}