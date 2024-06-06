using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using POS.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Repository
{
    public class TripList : List<TripDto>
    {

        IMapper _mapper;
        public TripList(IMapper mapper)
        {
            _mapper = mapper;
        }

        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }


        public TripList(List<TripDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);           
            AddRange(items);
        }

        public async Task<TripList> Create(IQueryable<Data.Trip> source, int skip, int pageSize)
        {

            var dtoList = await GetDtos(source, skip, pageSize);
            var count = pageSize == 0 || dtoList.Count() == 0 ? dtoList.Count() : await GetCount(source);
            
            var dtoPageList = new TripList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<Data.Trip> source)
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

        public async Task<List<TripDto>> GetDtos(IQueryable<Data.Trip> source, int skip, int pageSize)
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
                        TripType = cs.TripType,
                        //Purpose = _mapper.Map<PurposeDto>(cs.Purpose),
                        PurposeId = cs.PurposeId,                      
                        CreatedDate = cs.CreatedDate,
                        Approval = cs.Approval,
                        Status = cs.Status,
                        DepartmentId = cs.DepartmentId,
                        SourceCityId = cs.SourceCityId,
                        DestinationCityId = cs.DestinationCityId,
                        AdvanceMoney = cs.AdvanceMoney,
                        IsRequestAdvanceMoney = cs.IsRequestAdvanceMoney,
                        ModeOfTrip = cs.ModeOfTrip,
                        MultiCity= cs.MultiCity,
                        RequestAdvanceMoneyDate= cs.RequestAdvanceMoneyDate,
                        RequestAdvanceMoneyStatus=cs.RequestAdvanceMoneyStatus,
                        //Department = _mapper.Map<DepartmentDto>(cs.Department),
                        //DestinationCity = _mapper.Map<CityDto>(cs.DestinationCity),
                        //SourceCity = _mapper.Map<CityDto>(cs.SourceCity),
                        CreatedByUser=cs.CreatedByUser,
                        TripItinerarys=cs.TripItinerarys,
                        GradeName = cs.CreatedByUser.Grades.GradeName,
                        IsTripCompleted = cs.IsTripCompleted,
                        SourceCityName = cs.SourceCityName,
                        DestinationCityName = cs.DestinationCityName,
                        CompanyAccountId = cs.CompanyAccountId,
                        DepartmentName = cs.DepartmentName,
                        PurposeFor = cs.PurposeFor,
                        PendingItineraryApprove = cs.TripItinerarys.Where(a=>a.ApprovalStatus==null).Count(),
                        PendingHotelApprove = cs.TripHotelBookings.Where(a => a.ApprovalStatus == null).Count(),
                        TravelDocument = _mapper.Map<List<TravelDocumentDto>>(cs.CreatedByUser.TravelDocuments),
                        TripHotelBookings=cs.TripHotelBookings

                        // CreatedByUser = cs.CreatedByUser != null ? _mapper.Map<UserDto>(cs.CreatedByUser) : null,

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
                 TripType = cs.TripType,
                 //Purpose = _mapper.Map<PurposeDto>(cs.Purpose),
                 PurposeId = cs.PurposeId,
                 CreatedDate = cs.CreatedDate,
                 Approval=cs.Approval,
                 Status = cs.Status,
                 DepartmentId = cs.DepartmentId,
                 SourceCityId = cs.SourceCityId,
                 DestinationCityId = cs.DestinationCityId,
                 AdvanceMoney = cs.AdvanceMoney,
                 IsRequestAdvanceMoney = cs.IsRequestAdvanceMoney,
                 ModeOfTrip = cs.ModeOfTrip,
                 MultiCity = cs.MultiCity,
                 RequestAdvanceMoneyDate = cs.RequestAdvanceMoneyDate,
                 RequestAdvanceMoneyStatus = cs.RequestAdvanceMoneyStatus,
                // Department = _mapper.Map<DepartmentDto>(cs.Department),
                // DestinationCity = _mapper.Map<CityDto>(cs.DestinationCity),
                // SourceCity = _mapper.Map<CityDto>(cs.SourceCity),
                 CreatedByUser = cs.CreatedByUser,
                 TripItinerarys = cs.TripItinerarys,
                 GradeName = cs.CreatedByUser.Grades.GradeName,
                 IsTripCompleted = cs.IsTripCompleted,
                 SourceCityName = cs.SourceCityName,
                 DestinationCityName = cs.DestinationCityName,
                 CompanyAccountId = cs.CompanyAccountId,
                 DepartmentName = cs.DepartmentName,
                 PurposeFor= cs.PurposeFor,
                 PendingItineraryApprove = cs.TripItinerarys.Where(a => a.ApprovalStatus==null).Count(),
                 PendingHotelApprove = cs.TripHotelBookings.Where(a => a.ApprovalStatus==null).Count(),
                 TravelDocument = _mapper.Map<List<TravelDocumentDto>>(cs.CreatedByUser.TravelDocuments),
                 TripHotelBookings = cs.TripHotelBookings
                 // CreatedByUser = cs.CreatedByUser != null ? _mapper.Map<UserDto>(cs.CreatedByUser) : null,
             })
             .ToListAsync();
                return entities;
            }
        }

    }
}
