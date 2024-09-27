
using AutoMapper;
using Azure.Core;
using BTTEM.Data;
using BTTEM.Data.Resources;
using Microsoft.EntityFrameworkCore;
using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Data.Resources;
using POS.Domain;
using POS.Helper;
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
    public class PoliciesDetailRepository : GenericRepository<PoliciesDetail, POSDbContext>, IPoliciesDetailRepository
    {

        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly PathHelper _pathHelper;

        public PoliciesDetailRepository(IUnitOfWork<POSDbContext> uow,
            IPropertyMappingService propertyMappingService,
            IMapper mapper,
            PathHelper pathHelper,
            IUserRepository userRepository)
          : base(uow)
        {
            _propertyMappingService = propertyMappingService;
            _mapper = mapper;
            _pathHelper = pathHelper;
            _userRepository = userRepository;
        }

        public async Task<PoliciesDetailList> GetPoliciesDetails(PoliciesDetailResource policiesDetailResource)
        {
            var collectionBeforePaging =
                AllIncluding(c => c.Grade).Where(c => c.IsDeleted == false);

            if (!string.IsNullOrWhiteSpace(policiesDetailResource.Name))
            {
                // trim & ignore casing
                var genreForWhereClause = policiesDetailResource.Name
                    .Trim().ToLowerInvariant();
                var name = Uri.UnescapeDataString(genreForWhereClause);
                var encodingName = WebUtility.UrlDecode(name);
                var ecapestring = Regex.Unescape(encodingName);
                encodingName = encodingName.Replace(@"\", @"\\").Replace("%", @"\%").Replace("_", @"\_").Replace("[", @"\[").Replace(" ", "%");
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.Name, $"{encodingName}%"));
            }

            if (!string.IsNullOrWhiteSpace(policiesDetailResource.SearchQuery))
            {
                // trim & ignore casing
                var genreForWhereClause = policiesDetailResource.SearchQuery
                    .Trim().ToLowerInvariant();
                var name = Uri.UnescapeDataString(genreForWhereClause);
                var encodingName = WebUtility.UrlDecode(name);
                var ecapestring = Regex.Unescape(encodingName);
                encodingName = encodingName.Replace(@"\", @"\\").Replace("%", @"\%").Replace("_", @"\_").Replace("[", @"\[").Replace(" ", "%");
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.Name, $"{encodingName}%") ||
                    EF.Functions.Like(a.Grade.GradeName, $"{encodingName}%") ||
                    EF.Functions.Like(a.Grade.Description, $"{encodingName}%")
                    );
            }

            if (policiesDetailResource.Id.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Id == policiesDetailResource.Id);
            }
            if (policiesDetailResource.GradeId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.GradeId == policiesDetailResource.GradeId);
            }
            if (policiesDetailResource.CompanyAccountId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.CompanyAccountId == policiesDetailResource.CompanyAccountId);
            }
            var products = new PoliciesDetailList(_mapper, _pathHelper, _userRepository);
            return await products.Create(collectionBeforePaging, policiesDetailResource.Skip, policiesDetailResource.PageSize);
        }
    }
}
