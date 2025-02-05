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
    public class ApprovalLevelRepository : GenericRepository<ApprovalLevel, POSDbContext>, IApprovalLevelRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IMapper _mapper;
        public ApprovalLevelRepository(
            IUnitOfWork<POSDbContext> uow,
            IPropertyMappingService propertyMappingService,
             IMapper mapper)
            : base(uow)
        {
            _propertyMappingService = propertyMappingService;
            _mapper = mapper;
        }

        public async Task<ApprovalLevelList> GetApprovalLevels(ApprovalLevelResource approvalLevelResource)
        {
            var collectionBeforePaging =
                AllIncluding(l => l.ApprovalLevelType).ApplySort(approvalLevelResource.OrderBy,
                _propertyMappingService.GetPropertyMapping<ApprovalLevelDto, ApprovalLevel>());

            if (!string.IsNullOrEmpty(approvalLevelResource.LevelName))
            {
                // trim & ignore casing
                var genreForWhereClause = approvalLevelResource.LevelName
                    .Trim().ToLowerInvariant();
                var name = Uri.UnescapeDataString(genreForWhereClause);
                var encodingName = WebUtility.UrlDecode(name);
                var ecapestring = Regex.Unescape(encodingName);
                encodingName = encodingName.Replace(@"\", @"\\").Replace("%", @"\%").Replace("_", @"\_").Replace("[", @"\[").Replace(" ", "%");
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.LevelName, $"{encodingName}%"));
            }

            var ApprovalLevelList = new ApprovalLevelList(_mapper);
            return await ApprovalLevelList.Create(collectionBeforePaging, approvalLevelResource.Skip, approvalLevelResource.PageSize);
        }
    }
}