using AutoMapper;
using BTTEM.Data;
using Microsoft.EntityFrameworkCore;
using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using BTTEM.Data.Resources;
using POS.Domain;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BTTEM.Data.Dto;

namespace BTTEM.Repository.MMT
{
    public class MMTCityRepository : GenericRepository<MMTCity, POSDbContext>, IMMTCityRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;

        public MMTCityRepository(
            IUnitOfWork<POSDbContext> uow,
            IPropertyMappingService propertyMappingService,
             IMapper mapper)
            : base(uow)
        {
            _propertyMappingService = propertyMappingService;
        }
        public async Task<MMTCityList> GetMMTCities(MMTCityResource mmtCityResource)
        {
            var collectionBeforePaging =
                All.ApplySort(mmtCityResource.OrderBy,
                _propertyMappingService.GetPropertyMapping<MMTCityDto, MMTCity>());

            if (!string.IsNullOrEmpty(mmtCityResource.CityName))
            {
                // trim & ignore casing
                var genreForWhereClause = mmtCityResource.CityName
                    .Trim().ToLowerInvariant();
                var name = Uri.UnescapeDataString(genreForWhereClause);
                var encodingName = WebUtility.UrlDecode(name);
                var ecapestring = Regex.Unescape(encodingName);
                encodingName = encodingName.Replace(@"\", @"\\").Replace("%", @"\%").Replace("_", @"\_").Replace("[", @"\[").Replace(" ", "%");
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.CityName, $"{encodingName}%"));
            }
          
            var MMTCityList = new MMTCityList();
            return await MMTCityList.Create(collectionBeforePaging, mmtCityResource.Skip, mmtCityResource.PageSize);
        }
    }
}