using Microsoft.EntityFrameworkCore;
using POS.Data.Dto;
using POS.Data;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTTEM.Data.Dto;
using BTTEM.Data;
using POS.Helper;
using System.IO;

namespace BTTEM.Repository
{
    public class TripTrackingList : List<TripTrackingDto>
    {
        private PathHelper _pathHelper;
        public TripTrackingList(PathHelper pathHelper)
        {
            _pathHelper = pathHelper;
        }
        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public TripTrackingList(List<TripTrackingDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<TripTrackingList> Create(IQueryable<TripTracking> source, int skip, int pageSize)
        {
            var count = await GetCount(source);
            var dtoList = await GetDtos(source, skip, pageSize);
            var dtoPageList = new TripTrackingList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<TripTracking> source)
        {
            return await source.AsNoTracking().CountAsync();
        }

        public async Task<List<TripTrackingDto>> GetDtos(IQueryable<TripTracking> source, int skip, int pageSize)
        {
            var entities = await source
                .Skip(skip)
                .Take(pageSize)
                .AsNoTracking()
                .Select(c => new TripTrackingDto
                {
                    Id = c.Id,
                    TripId = c.TripId,
                    TripItineraryId = c.TripItineraryId,
                    ActionType = c.ActionType,
                    Status = c.Status,
                    Remarks = c.Remarks,
                    ActionBy = c.ActionBy,
                    ActionByName = c.ActionByUser.FirstName + " " + c.ActionByUser.LastName,
                    ProfilePhoto = !string.IsNullOrWhiteSpace(c.ActionByUser.ProfilePhoto) ? Path.Combine(_pathHelper.UserProfilePath, c.ActionByUser.ProfilePhoto) : "",                    
                    ActionDate = c.ActionDate,
                    TripTypeName = c.TripTypeName,
                    ImageUrl = c.ImageUrl,

                }).ToListAsync();
            return entities;
        }
    }
}