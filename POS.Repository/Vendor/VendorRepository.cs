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
    public class VendorRepository : GenericRepository<Vendor, POSDbContext>, IVendorRepository
    {
        public IPropertyMappingService _propertyMappingService { get; set; }
        public VendorRepository(IUnitOfWork<POSDbContext> uow,
            IPropertyMappingService propertyMappingService,
            IMapper _mapper) : base(uow) {

            _propertyMappingService = propertyMappingService;
        }

        public async Task<VendorList> GetVendors(VendorResource vendorResource)
        {
            var collectionBeforePaging =
               AllIncluding().ApplySort(vendorResource.OrderBy,
               _propertyMappingService.GetPropertyMapping<VendorDto, Vendor>());

            if (!string.IsNullOrEmpty(vendorResource.VendorName))
            {
                // trim & ignore casing
                var genreForWhereClause = vendorResource.VendorName
                    .Trim().ToLowerInvariant();
                var name = Uri.UnescapeDataString(genreForWhereClause);
                var encodingName = WebUtility.UrlDecode(name);
                var ecapestring = Regex.Unescape(encodingName);
                encodingName = encodingName.Replace(@"\", @"\\").Replace("%", @"\%").Replace("_", @"\_").Replace("[", @"\[").Replace(" ", "%");
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.VendorName, $"{encodingName}%"));
            }

            if (!string.IsNullOrEmpty(vendorResource.VendorCode))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.VendorCode, $"%{vendorResource.VendorCode}%"));
            }


                var VendorList = new VendorList();
            return await VendorList.Create(collectionBeforePaging, vendorResource.Skip, vendorResource.PageSize);
        }
    }
}
