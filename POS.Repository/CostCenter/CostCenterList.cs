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
    public class CostCenterList : List<CostCenterDto>
    {
        public CostCenterList()
        {

        }

        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public CostCenterList(List<CostCenterDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<CostCenterList> Create(IQueryable<CostCenter> source, int skip, int pageSize)
        {
            var count = await GetCount(source);
            var dtoList = await GetDtos(source, skip, pageSize);
            var dtoPageList = new CostCenterList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<CostCenter> source)
        {
            return await source.AsNoTracking().CountAsync();
        }
        public async Task<List<CostCenterDto>> GetDtos(IQueryable<CostCenter> source, int skip, int pageSize)
        {
            if (pageSize == 0)
            {
                var entities = await source
               .Skip(skip)
               .AsNoTracking()
               .Select(c => new CostCenterDto
               {
                   Id = c.Id,
                   CostCenterBranchName = c.CostCenterBranchName,
                   BusinessArea = c.BusinessArea,
                   BusinessPlace = c.BusinessPlace,
                   ProfitCenter = c.ProfitCenter,
               }).ToListAsync();
                return entities;
            }
            else
            {
                var entities = await source
               .Skip(skip)
               //.Take(pageSize)
               .AsNoTracking()
               .Select(c => new CostCenterDto
               {
                   Id = c.Id,
                   CostCenterBranchName = c.CostCenterBranchName,
                   BusinessArea = c.BusinessArea,
                   BusinessPlace= c.BusinessPlace,
                   ProfitCenter = c.ProfitCenter,
               }).ToListAsync();
                return entities;
            }
        }
    }
}