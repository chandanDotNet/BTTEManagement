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
    public class TravelDocumentRepository : GenericRepository<TravelDocument, POSDbContext>, ITravelDocumentRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;
        public TravelDocumentRepository(IUnitOfWork<POSDbContext> uow,
            IPropertyMappingService propertyMappingService,
            IMapper mapper)
        : base(uow)
        {
            _propertyMappingService = propertyMappingService;
        }

        public async Task<TravelDocumentList> GetTravelDocuments(TravelDocumentResource travelDocumentResource)
        {
            var collectionBeforePaging =
                All.ApplySort(travelDocumentResource.OrderBy,
                _propertyMappingService.GetPropertyMapping<TravelDocumentDto, TravelDocument>());

            if (!string.IsNullOrEmpty(travelDocumentResource.SearchQuery))
            {
                // trim & ignore casing
                var genreForWhereClause = travelDocumentResource.SearchQuery
                    .Trim().ToLowerInvariant();
                var name = Uri.UnescapeDataString(genreForWhereClause);
                var encodingName = WebUtility.UrlDecode(name);
                var ecapestring = Regex.Unescape(encodingName);
                encodingName = encodingName.Replace(@"\", @"\\").Replace("%", @"\%").Replace("_", @"\_").Replace("[", @"\[").Replace(" ", "%");
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.DocNumber, $"{encodingName}%"));
            }

            if (travelDocumentResource.UserId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                  .Where(a => a.UserId == travelDocumentResource.UserId);
            }

            //if (!string.IsNullOrEmpty(tripTrackingResource.ActionType))
            //{
            //    collectionBeforePaging = collectionBeforePaging
            //      .Where(a => a.ActionType == tripTrackingResource.ActionType);
            //}

            var TravelDocumentList = new TravelDocumentList();
            return await TravelDocumentList.Create(collectionBeforePaging, travelDocumentResource.Skip, travelDocumentResource.PageSize);
        }
    }
}
