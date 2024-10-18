using AutoMapper;
using BTTEM.Data.Dto;
using BTTEM.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Repository
{
    public class AdvanceMoneyList : List<TripDto>
    {
        IMapper _mapper;
        public AdvanceMoneyList(IMapper mapper)
        {
            _mapper = mapper;
        }

        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }


        public AdvanceMoneyList(List<TripDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<AdvanceMoneyList> Create(IQueryable<Data.Trip> source, int skip, int pageSize)
        {

            var dtoList = await GetDtos(source, skip, pageSize);
            var count = pageSize == 0 || dtoList.Count() == 0 ? dtoList.Count() : await GetCount(source);

            var dtoPageList = new AdvanceMoneyList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<Trip> source)
        {
            try
            {
                return await source.AsNoTracking().CountAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<TripDto>> GetDtos(IQueryable<Trip> source, int skip, int pageSize)
        {
            if (pageSize == 0)
            {
                var entities = await source
                    .AsNoTracking()
                    .Select(cs => new TripDto
                    {
                        Id = cs.Id,
                        Description = cs.Description,
                        TripNo = cs.TripNo,
                        Name = cs.Name,
                        TripStarts = cs.TripStarts,
                        TripEnds = cs.TripEnds,
                        TotalDays = (cs.TripEnds - cs.TripStarts).Days,
                        TripType = cs.TripType,                       
                        CreatedDate = cs.CreatedDate,
                        Approval = cs.Approval,
                        Status = cs.Status,
                        DepartmentId = cs.DepartmentId,
                        AdvanceMoney = cs.AdvanceMoney,
                        AdvanceMoneyApprovedAmount= cs.AdvanceMoneyApprovedAmount,
                        IsRequestAdvanceMoney = cs.IsRequestAdvanceMoney,
                        ModeOfTrip = cs.ModeOfTrip,
                        MultiCity = cs.MultiCity,
                        RequestAdvanceMoneyDate = cs.RequestAdvanceMoneyDate,
                        RequestAdvanceMoneyStatus = cs.RequestAdvanceMoneyStatus,
                        AdvanceMoneyRemarks = cs.AdvanceMoneyRemarks,
                        CreatedByUser = cs.CreatedByUser,                       
                        GradeName = cs.CreatedByUser.Grades.GradeName,
                        BranchName = cs.CreatedByUser.CompanyAccountBranch.Name,
                        IsTripCompleted = cs.IsTripCompleted,
                        SourceCityName = cs.SourceCityName,
                        DestinationCityName = cs.DestinationCityName,
                        CompanyAccountId = cs.CompanyAccountId,
                        DepartmentName = cs.DepartmentName,                       
                        TripHotelBookings = cs.TripHotelBookings,
                        RequestAdvanceMoneyStatusBys = cs.RequestAdvanceMoneyStatusBys,
                        RequestAdvanceMoneyStatusBy = cs.RequestAdvanceMoneyStatusBy,
                        CancellationDateTime = cs.CancellationDateTime,
                        CancellationConfirmation = cs.CancellationConfirmation,
                        CancellationReason = cs.CancellationReason,
                        TravelDeskId = cs.TravelDeskId,
                        TravelDeskName = cs.TravelDeskName,
                        JourneyNumber = cs.JourneyNumber,
                        IsGroupTrip = cs.IsGroupTrip,
                        NoOfPerson = cs.NoOfPerson,
                        Consent = cs.Consent,
                        IsGroupTripCancelRequest = cs.IsGroupTripCancelRequest == null ? false : cs.IsGroupTripCancelRequest,
                        GroupTrips = _mapper.Map<List<GroupTripDto>>(cs.GroupTrips),
                        CompanyAccount = _mapper.Map<CompanyAccountDto>(cs.CreatedByUser.CompanyAccounts)
                    })
                    .ToListAsync();
                return entities;
            }
            else
            {
                var entities = await source
             .Skip(skip)
             .Take(pageSize)
             .AsNoTracking()
             .Select(cs => new TripDto
             {
                 Id = cs.Id,
                 Description = cs.Description,
                 TripNo = cs.TripNo,
                 Name = cs.Name,
                 TripStarts = cs.TripStarts,
                 TripEnds = cs.TripEnds,
                 TotalDays = (cs.TripEnds - cs.TripStarts).Days,
                 TripType = cs.TripType,
                 CreatedDate = cs.CreatedDate,
                 Approval = cs.Approval,
                 Status = cs.Status,
                 DepartmentId = cs.DepartmentId,
                 AdvanceMoney = cs.AdvanceMoney,
                 AdvanceMoneyApprovedAmount = cs.AdvanceMoneyApprovedAmount,
                 IsRequestAdvanceMoney = cs.IsRequestAdvanceMoney,
                 ModeOfTrip = cs.ModeOfTrip,
                 MultiCity = cs.MultiCity,
                 RequestAdvanceMoneyDate = cs.RequestAdvanceMoneyDate,
                 RequestAdvanceMoneyStatus = cs.RequestAdvanceMoneyStatus,
                 AdvanceMoneyRemarks = cs.AdvanceMoneyRemarks,
                 CreatedByUser = cs.CreatedByUser,
                 GradeName = cs.CreatedByUser.Grades.GradeName,
                 BranchName = cs.CreatedByUser.CompanyAccountBranch.Name,
                 IsTripCompleted = cs.IsTripCompleted,
                 SourceCityName = cs.SourceCityName,
                 DestinationCityName = cs.DestinationCityName,
                 CompanyAccountId = cs.CompanyAccountId,
                 DepartmentName = cs.DepartmentName,
                 TripHotelBookings = cs.TripHotelBookings,
                 RequestAdvanceMoneyStatusBys = cs.RequestAdvanceMoneyStatusBys,
                 RequestAdvanceMoneyStatusBy = cs.RequestAdvanceMoneyStatusBy,
                 CancellationDateTime = cs.CancellationDateTime,
                 CancellationConfirmation = cs.CancellationConfirmation,
                 CancellationReason = cs.CancellationReason,
                 TravelDeskId = cs.TravelDeskId,
                 TravelDeskName = cs.TravelDeskName,
                 JourneyNumber = cs.JourneyNumber,
                 IsGroupTrip = cs.IsGroupTrip,
                 NoOfPerson = cs.NoOfPerson,
                 Consent = cs.Consent,
                 IsGroupTripCancelRequest = cs.IsGroupTripCancelRequest == null ? false : cs.IsGroupTripCancelRequest,
                 GroupTrips = _mapper.Map<List<GroupTripDto>>(cs.GroupTrips),
                 CompanyAccount = _mapper.Map<CompanyAccountDto>(cs.CreatedByUser.CompanyAccounts)
             })
             .ToListAsync();
                return entities;
            }
        }
    }
}
