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
using BTTEM.Data;
using BTTEM.Data.Resources;
using BTTEM.Data.Dto;

namespace BTTEM.Repository
{
    public class ApprovalLevelTypeRepository : GenericRepository<ApprovalLevelType, POSDbContext>, IApprovalLevelTypeRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;

        public ApprovalLevelTypeRepository(
            IUnitOfWork<POSDbContext> uow,
            IPropertyMappingService propertyMappingService,
             IMapper mapper)
            : base(uow)
        {
            _propertyMappingService = propertyMappingService;
        }

        public async Task<ApprovalLevelTypeList> GetApprovalLevelTypes(ApprovalLevelTypeResource approvalLevelTypeResource)
        {
            var collectionBeforePaging =
                AllIncluding(c => c.Company).ApplySort(approvalLevelTypeResource.OrderBy,
                _propertyMappingService.GetPropertyMapping<ApprovalLevelTypeDto, ApprovalLevelType>());

            if (!string.IsNullOrEmpty(approvalLevelTypeResource.TypeName))
            {
                // trim & ignore casing
                var genreForWhereClause = approvalLevelTypeResource.TypeName
                    .Trim().ToLowerInvariant();
                var name = Uri.UnescapeDataString(genreForWhereClause);
                var encodingName = WebUtility.UrlDecode(name);
                var ecapestring = Regex.Unescape(encodingName);
                encodingName = encodingName.Replace(@"\", @"\\").Replace("%", @"\%").Replace("_", @"\_").Replace("[", @"\[").Replace(" ", "%");
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.TypeName, $"{encodingName}%"));
            }

            if (!string.IsNullOrEmpty(approvalLevelTypeResource.CompanyName))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Company.AccountName == approvalLevelTypeResource.CompanyName);
            }

            var ApprovalLevelList = new ApprovalLevelTypeList();
            return await ApprovalLevelList.Create(collectionBeforePaging, approvalLevelTypeResource.Skip, approvalLevelTypeResource.PageSize);
        }
    }
}