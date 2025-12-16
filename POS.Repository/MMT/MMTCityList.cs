using Microsoft.EntityFrameworkCore;
using BTTEM.Data;
using BTTEM.Data.Dto;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Repository
{
    public class MMTCityList : List<MMTCityDto>
    {
        public MMTCityList()
        {
        }
        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public MMTCityList(List<MMTCityDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<MMTCityList> Create(IQueryable<MMTCity> source, int skip, int pageSize)
        {
            var count = await GetCount(source);
            var dtoList = await GetDtos(source, skip, pageSize);
            var dtoPageList = new MMTCityList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<MMTCity> source)
        {
            return await source.AsNoTracking().CountAsync();
        }

        public async Task<List<MMTCityDto>> GetDtos(IQueryable<MMTCity> source, int skip, int pageSize)
        {

            if (pageSize == 0)
            {
                var entities = await source
               .Skip(skip)
               .AsNoTracking()
               .Select(c => new MMTCityDto
               {
                   Id = c.Id,
                   CityName = c.CityName,
                   CityCode = c.CityCode,
                   StateId = c.StateId,
                   StateName = c.StateName

               }).ToListAsync();
                return entities;
            }
            else
            {
                var entities = await source
               .Skip(skip)
               .Take(pageSize)
               .AsNoTracking()
               .Select(c => new MMTCityDto
               {
                   Id = c.Id,
                   CityName = c.CityName,
                   CityCode = c.CityCode,
                   StateId = c.StateId,
                   StateName = c.StateName

               }).ToListAsync();
                return entities;
            }
        }
    }
}