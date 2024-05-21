using AutoMapper;
using BTTEM.Data;
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
using BTTEM.Data.Dto;

namespace BTTEM.Repository
{
    public class TripTrackingRepository : GenericRepository<TripTracking, POSDbContext>, ITripTrackingRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;
        public TripTrackingRepository(IUnitOfWork<POSDbContext> uow,
             IPropertyMappingService propertyMappingService,
             IMapper mapper)
        : base(uow)
        {
            _propertyMappingService = propertyMappingService;
        }

        public async Task<TripTrackingList> GetTripTrackings(TripTrackingResource tripTrackingResource)
        {
            var collectionBeforePaging =
                AllIncluding(r => r.ActionByUser).ApplySort(tripTrackingResource.OrderBy,
                _propertyMappingService.GetPropertyMapping<TripTrackingDto, TripTracking>());

            //if (!string.IsNullOrEmpty(cityResource.CityName))
            //{
            //    // trim & ignore casing
            //    var genreForWhereClause = cityResource.CityName
            //        .Trim().ToLowerInvariant();
            //    var name = Uri.UnescapeDataString(genreForWhereClause);
            //    var encodingName = WebUtility.UrlDecode(name);
            //    var ecapestring = Regex.Unescape(encodingName);
            //    encodingName = encodingName.Replace(@"\", @"\\").Replace("%", @"\%").Replace("_", @"\_").Replace("[", @"\[").Replace(" ", "%");
            //    collectionBeforePaging = collectionBeforePaging
            //        .Where(a => EF.Functions.Like(a.CityName, $"{encodingName}%"));
            //}

            if (tripTrackingResource.TripId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                  .Where(a => a.TripId == tripTrackingResource.TripId);
            }

            if (tripTrackingResource.TripItineraryId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                  .Where(a => a.TripItineraryId == tripTrackingResource.TripItineraryId);
            }

            if (!string.IsNullOrEmpty(tripTrackingResource.ActionType))
            {
                collectionBeforePaging = collectionBeforePaging
                  .Where(a => a.ActionType == tripTrackingResource.ActionType);
            }

            var TripTrackingList = new TripTrackingList();
            return await TripTrackingList.Create(collectionBeforePaging, tripTrackingResource.Skip, tripTrackingResource.PageSize);
        }
    }
}
