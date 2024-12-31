using AutoMapper;
using BTTEM.Data.Resources;
using BTTEM.Data;
using Microsoft.EntityFrameworkCore;
using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace BTTEM.Repository
{
    public class AdvanceMoneyRepository : GenericRepository<Trip, POSDbContext>, IAdvanceMoneyRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IMapper _mapper;
        private readonly UserInfoToken _userInfoToken;
        private readonly IUserRepository _userRepository;
        private IConfiguration _configuration;
        public AdvanceMoneyRepository(IUnitOfWork<POSDbContext> uow,
            IPropertyMappingService propertyMappingService,
              IMapper mapper,
              UserInfoToken userInfoToken,
              IUserRepository userRepository,
              IConfiguration configuration
            ) : base(uow)
        {
            _propertyMappingService = propertyMappingService;
            _mapper = mapper;
            _userInfoToken = userInfoToken;
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<AdvanceMoneyList> GetAllAdvanceMoney(AdvanceMoneyResource advanceMoneyResource)
        {
            decimal amount = Convert.ToDecimal(this._configuration.GetSection("AdvanceMoney")["Money"]);
            Guid LoginUserId = Guid.Parse(_userInfoToken.Id);

            var collectionBeforePaging = All.
                 Include(c => c.CreatedByUser)
                .Include(ti => ti.TripItinerarys)
                .Include(t => t.TripHotelBookings)
                .Include(a => a.RequestAdvanceMoneyStatusBys)
                .Include(e => e.GroupTrips)
                .ThenInclude(c => c.User)
                .ThenInclude(g => g.Grades)
                .Include(s => s.GroupTrips)
                .ThenInclude(x => x.User)
                .ThenInclude(c => c.CompanyAccounts)
                .Include(g => g.CreatedByUser.Grades)
                .Include(a => a.CreatedByUser.CompanyAccounts)
                .Include(k => k.ApprovedBy)
                .ApplySort(advanceMoneyResource.OrderBy,
               _propertyMappingService.GetPropertyMapping<TripDto, Trip>());

            if (LoginUserId == Guid.Parse("fe7c8f30-965c-4f12-9eca-c00f9d4f99a4"))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.AdvanceMoney >= amount 
                    && a.CompanyAccountId == Guid.Parse("d0ccea5f-5393-4a34-9df6-43a9f51f9f91")
                    || a.ProjectType == "Others");
            }

            if (LoginUserId == Guid.Parse("6162414f-06fd-4460-b447-2499aa88c602"))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.AdvanceMoney < amount &&
                    a.CompanyAccountId == Guid.Parse("d0ccea5f-5393-4a34-9df6-43a9f51f9f91") 
                    || a.ProjectType == "Ongoing");
            }
            if (!string.IsNullOrEmpty(advanceMoneyResource.RequestAdvanceMoneyStatus))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.AdvanceMoney < amount && a.RequestAdvanceMoneyStatus == advanceMoneyResource.RequestAdvanceMoneyStatus);
            }

            if (!string.IsNullOrEmpty(advanceMoneyResource.ProjectType))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.ProjectType == advanceMoneyResource.ProjectType);
            }

            //Filter For Infra Only           

            //if (advanceMoneyResource.DepartmentId.HasValue)
            //{
            //    collectionBeforePaging = collectionBeforePaging
            //        .Where(a => a.DepartmentId == advanceMoneyResource.DepartmentId);
            //}

            //if (tripResource.PurposeId.HasValue)
            //{
            //    collectionBeforePaging = collectionBeforePaging
            //        .Where(a => a.PurposeId == tripResource.PurposeId);
            //}

            //if (!string.IsNullOrEmpty(tripResource.Name))
            //{
            //    collectionBeforePaging = collectionBeforePaging
            //        .Where(a => a.Name == tripResource.Name);
            //}
            //if (!string.IsNullOrEmpty(tripResource.TripType))
            //{
            //    collectionBeforePaging = collectionBeforePaging
            //        .Where(a => a.TripType == tripResource.TripType);
            //}
            //if (!string.IsNullOrEmpty(tripResource.TripNo))
            //{
            //    collectionBeforePaging = collectionBeforePaging
            //        .Where(a => a.TripNo == tripResource.TripNo);
            //}
            //if (!string.IsNullOrEmpty(tripResource.BookTypeBy))
            //{
            //    collectionBeforePaging = collectionBeforePaging
            //        .Where(a => a.TripItinerarys.Any(a => a.BookTypeBy == tripResource.BookTypeBy));
            //}
            //if (!string.IsNullOrEmpty(tripResource.Status))
            //{
            //    collectionBeforePaging = collectionBeforePaging
            //        .Where(a => a.Status == tripResource.Status);
            //}
            //if (!string.IsNullOrEmpty(tripResource.Approval))
            //{
            //    collectionBeforePaging = collectionBeforePaging
            //        .Where(a => a.Approval == tripResource.Approval);
            //}
            //if (!string.IsNullOrEmpty(tripResource.IsTripCompleted))
            //{
            //    if (tripResource.IsTripCompleted == "True")
            //    {
            //        collectionBeforePaging = collectionBeforePaging
            //            .Where(a => a.IsTripCompleted == true);
            //    }
            //    if (tripResource.IsTripCompleted == "False")
            //    {
            //        collectionBeforePaging = collectionBeforePaging
            //            .Where(a => a.IsTripCompleted == false);
            //    }
            //}

            //if (!string.IsNullOrEmpty(tripResource.PurposeFor))
            //{
            //    collectionBeforePaging = collectionBeforePaging
            //        .Where(a => a.PurposeFor == tripResource.PurposeFor);
            //}
            //if (!string.IsNullOrEmpty(tripResource.SourceCityName))
            //{
            //    collectionBeforePaging = collectionBeforePaging
            //        .Where(a => a.SourceCityName == tripResource.SourceCityName);
            //}
            //if (!string.IsNullOrEmpty(tripResource.DestinationCityName))
            //{
            //    collectionBeforePaging = collectionBeforePaging
            //        .Where(a => a.DestinationCityName == tripResource.DestinationCityName);
            //}
            //if (!string.IsNullOrEmpty(tripResource.DepartmentName))
            //{
            //    collectionBeforePaging = collectionBeforePaging
            //        .Where(a => a.DepartmentName == tripResource.DepartmentName);
            //}
            //if (tripResource.BillingCompanyAccountId.HasValue)
            //{
            //    collectionBeforePaging = collectionBeforePaging
            //        .Where(a => a.CompanyAccountId == tripResource.BillingCompanyAccountId);
            //}

            //if (tripResource.FromDate.HasValue)
            //{
            //    collectionBeforePaging = collectionBeforePaging
            //        .Where(a => a.TripStarts >= new DateTime(tripResource.FromDate.Value.Year, tripResource.FromDate.Value.Month, tripResource.FromDate.Value.Day, 0, 0, 1));
            //}
            //if (tripResource.ToDate.HasValue)
            //{
            //    collectionBeforePaging = collectionBeforePaging
            //        .Where(a => a.TripEnds <= new DateTime(tripResource.ToDate.Value.Year, tripResource.ToDate.Value.Month, tripResource.ToDate.Value.Day, 23, 59, 59));
            //}

            //if (!string.IsNullOrEmpty(tripResource.BranchName))
            //{
            //    collectionBeforePaging = collectionBeforePaging
            //        .Where(a => a.CreatedByUser.BranchName == tripResource.BranchName);
            //}

            //if (tripResource.BranchId.HasValue)
            //{
            //    collectionBeforePaging = collectionBeforePaging
            //        .Where(a => a.CreatedByUser.CompanyAccountBranchId == tripResource.BranchId);
            //}

            //if (!string.IsNullOrEmpty(tripResource.SearchQuery))
            //{
            //    var searchQueryForWhereClause = tripResource.SearchQuery
            //  .Trim().ToLowerInvariant();
            //    collectionBeforePaging = collectionBeforePaging
            //        .Where(a =>
            //        EF.Functions.Like(a.TripNo, $"%{searchQueryForWhereClause}%")
            //        || EF.Functions.Like(a.Name, $"%{searchQueryForWhereClause}%")
            //        || EF.Functions.Like(a.TripType, $"%{searchQueryForWhereClause}%")
            //        || EF.Functions.Like(a.ModeOfTrip, $"%{searchQueryForWhereClause}%")
            //        || EF.Functions.Like(a.Status, $"%{searchQueryForWhereClause}%")
            //        || EF.Functions.Like(a.SourceCityName, $"%{searchQueryForWhereClause}%")
            //        || EF.Functions.Like(a.DestinationCityName, $"%{searchQueryForWhereClause}%")
            //        || EF.Functions.Like(a.Approval, $"%{searchQueryForWhereClause}%")
            //        //|| EF.Functions.Like(a.TripStarts.ToShortDateString(), $"%{searchQueryForWhereClause}%")                    
            //        //|| EF.Functions.Like(a.TripEnds.ToShortDateString(), $"%{searchQueryForWhereClause}%")                    
            //        );
            //}

            return await new AdvanceMoneyList(_mapper).Create(collectionBeforePaging,
                advanceMoneyResource.Skip,
                advanceMoneyResource.PageSize);
        }
    }
}