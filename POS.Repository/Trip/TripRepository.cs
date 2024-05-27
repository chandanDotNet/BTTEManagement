using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Resources;
using BTTEM.Repository.Expense;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Data.Resources;
using POS.Domain;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Repository
{
    public class TripRepository : GenericRepository<Trip, POSDbContext>, ITripRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IMapper _mapper;
        private readonly UserInfoToken _userInfoToken;
        private readonly IUserRoleRepository _userRoleRepository;
        public TripRepository(IUnitOfWork<POSDbContext> uow,
            IPropertyMappingService propertyMappingService,
            IMapper mapper,
             UserInfoToken userInfoToken,
              IUserRoleRepository userRoleRepository
            ) : base(uow)
        {
            _propertyMappingService = propertyMappingService;
            _mapper = mapper;
            _userInfoToken = userInfoToken;
            _userRoleRepository = userRoleRepository;
        }

        public async Task<TripList> GetAllTrips(TripResource tripResource)
        {
            Guid LoginUserId = Guid.Parse(_userInfoToken.Id);
            var Role = GetUserRole(LoginUserId).Result.FirstOrDefault();
            if(Role != null)
            {
                if(Role.Id ==new Guid("F9B4CCD2-6E06-443C-B964-23BF935F859E")) //Reporting Manager
                {
                    tripResource.ReportingHeadId = LoginUserId;
                }
                //else if (Role.Id == new Guid("F72616BE-260B-41BB-A4EE-89146622179A")) //Travel Desk
                //{
                //    tripResource.ReportingHeadId = null;
                //}
                else if (Role.Id == new Guid("E1BD3DCE-EECF-468D-B930-1875BD59D1F4")) //Submitter
                {
                    tripResource.CreatedBy=LoginUserId;
                }
            }
            //var collectionBeforePaging = AllIncluding(c => c.CreatedByUser).ApplySort(expenseResource.OrderBy,
            //    _propertyMappingService.GetPropertyMapping<MasterExpenseDto, MasterExpense>());
            var collectionBeforePaging = AllIncluding(c => c.CreatedByUser, a => a.Department,b=>b.SourceCity,d=>d.DestinationCity,ti=>ti.TripItinerarys).ApplySort(tripResource.OrderBy,
                _propertyMappingService.GetPropertyMapping<TripDto, Trip>());
            //.ProjectTo<TripDto>(_mapper.ConfigurationProvider).ToListAsync();

            if (tripResource.ReportingHeadId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.CreatedByUser.ReportingTo == tripResource.ReportingHeadId);
            }
            if (tripResource.CompanyAccountId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.CreatedByUser.CompanyAccountId == tripResource.CompanyAccountId);
            }
            if (tripResource.Id.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Id == tripResource.Id);
            }

            if (tripResource.CreatedBy.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.CreatedBy == tripResource.CreatedBy);
            }

            if (tripResource.DepartmentId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.DepartmentId == tripResource.DepartmentId);
            }

            if (tripResource.PurposeId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.PurposeId == tripResource.PurposeId);
            }
            
            if (!string.IsNullOrEmpty(tripResource.Name))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Name == tripResource.Name);
            }
            if (!string.IsNullOrEmpty(tripResource.TripType))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.TripType == tripResource.TripType);
            }
            if (!string.IsNullOrEmpty(tripResource.TripNo))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.TripNo == tripResource.TripNo);
            }
            if (!string.IsNullOrEmpty(tripResource.BookTypeBy))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.TripItinerarys.Any(a=>a.BookTypeBy== tripResource.BookTypeBy));
            }
            if (!string.IsNullOrEmpty(tripResource.Status))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Status == tripResource.Status);
            }
            if (!string.IsNullOrEmpty(tripResource.Approval))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Approval == tripResource.Approval);
            }
            if (!string.IsNullOrEmpty(tripResource.IsTripCompleted))
            {
                if(tripResource.IsTripCompleted == "True")
                {
                    collectionBeforePaging = collectionBeforePaging
                        .Where(a => a.IsTripCompleted == true);
                }
                if (tripResource.IsTripCompleted == "False")
                {
                    collectionBeforePaging = collectionBeforePaging
                        .Where(a => a.IsTripCompleted == false);
                }
            }
            

            if (!string.IsNullOrEmpty(tripResource.SearchQuery))
            {
                var searchQueryForWhereClause = tripResource.SearchQuery
              .Trim().ToLowerInvariant();
                collectionBeforePaging = collectionBeforePaging
                    .Where(a =>
                    EF.Functions.Like(a.TripNo, $"%{searchQueryForWhereClause}%")
                    || EF.Functions.Like(a.Name, $"%{searchQueryForWhereClause}%")
                    || EF.Functions.Like(a.TripType, $"%{searchQueryForWhereClause}%")
                    || EF.Functions.Like(a.ModeOfTrip, $"%{searchQueryForWhereClause}%")
                    || EF.Functions.Like(a.Status, $"%{searchQueryForWhereClause}%")                    
                    );
            }

            

            return await new TripList(_mapper).Create(collectionBeforePaging,
                tripResource.Skip,
                tripResource.PageSize);
        }


        public async Task<List<RoleDto>> GetUserRole(Guid Id)
        {
            var rolesDetails = await _userRoleRepository.AllIncluding(c => c.Role).Where(d => d.UserId == Id)
                .ToListAsync();

            List<RoleDto> roleDto = new List<RoleDto>();
            foreach (var role in rolesDetails)
            {
                RoleDto rd = new RoleDto();
                rd.Id = role.Role.Id;
                rd.Name = role.Role.Name;
                roleDto.Add(rd);
            }
            // var roleClaims = await _roleClaimRepository.All.Where(c => rolesIds.Contains(c.RoleId)).Select(c => c.ClaimType).ToListAsync();
            return roleDto;
        }
    }
}
