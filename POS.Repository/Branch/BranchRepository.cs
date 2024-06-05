using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.Data.Resources;
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
    public class BranchRepository : GenericRepository<Branch, POSDbContext>, IBranchRepository
    {
        public IPropertyMappingService _propertyMappingService { get; set; }
        public BranchRepository(
            IUnitOfWork<POSDbContext> uow,
            IPropertyMappingService propertyMappingService,
            IMapper _mapper) : base(uow)
        {
            _propertyMappingService = propertyMappingService;
        }

        public async Task<BranchList> GetBranches(BranchResource branchResource)
        {
            var collectionBeforePaging =
               AllIncluding().ApplySort(branchResource.OrderBy,
               _propertyMappingService.GetPropertyMapping<BranchDto, Branch>());

            if (!string.IsNullOrEmpty(branchResource.Name))
            {
                // trim & ignore casing
                var genreForWhereClause = branchResource.Name
                    .Trim().ToLowerInvariant();
                var name = Uri.UnescapeDataString(genreForWhereClause);
                var encodingName = WebUtility.UrlDecode(name);
                var ecapestring = Regex.Unescape(encodingName);
                encodingName = encodingName.Replace(@"\", @"\\").Replace("%", @"\%").Replace("_", @"\_").Replace("[", @"\[").Replace(" ", "%");
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.Name, $"{encodingName}%"));
            }

            var BranchList = new BranchList();
            return await BranchList.Create(collectionBeforePaging, branchResource.Skip, branchResource.PageSize);
        }
    }
}
