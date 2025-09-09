using BTTEM.Data;
using BTTEM.Data.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Repository
{
    public class BusinessAreaList : List<BusinessAreaDto>
    {
        public BusinessAreaList()
        {

        }

        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public BusinessAreaList(List<BusinessAreaDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<BusinessAreaList> Create(IQueryable<BusinessArea> source, int skip, int pageSize)
        {
            var count = await GetCount(source);
            var dtoList = await GetDtos(source, skip, pageSize);
            var dtoPageList = new BusinessAreaList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<BusinessArea> source)
        {
            return await source.AsNoTracking().CountAsync();
        }
        public async Task<List<BusinessAreaDto>> GetDtos(IQueryable<BusinessArea> source, int skip, int pageSize)
        {
            if (pageSize == 0)
            {
                var entities = await source
               .Skip(skip)
               .AsNoTracking()
               .Select(c => new BusinessAreaDto
               {
                   Id = c.Id,
                   CostCenterBranchName = c.CostCenterBranchName,
                   BusinessAreaStateName = c.BusinessAreaStateName,
                   BusinessAreaName = c.BusinessAreaName,
                   BusinessPlace = c.BusinessPlace,
                   ProfitCenter = c.ProfitCenter,
                   CompanyAccountId = c.CompanyAccountId
               }).ToListAsync();
                return entities;
            }
            else
            {
                var entities = await source
               .Skip(skip)
               //.Take(pageSize)
               .AsNoTracking()
               .Select(c => new BusinessAreaDto
               {
                   Id = c.Id,
                   CostCenterBranchName = c.CostCenterBranchName,
                   BusinessAreaStateName = c.BusinessAreaStateName,
                   BusinessAreaName = c.BusinessAreaName,
                   BusinessPlace = c.BusinessPlace,
                   ProfitCenter = c.ProfitCenter,
                   CompanyAccountId = c.CompanyAccountId
               }).ToListAsync();
                return entities;
            }
        }
    }
}