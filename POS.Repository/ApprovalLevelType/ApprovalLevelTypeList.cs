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

namespace BTTEM.Repository
{
    public class ApprovalLevelTypeList : List<ApprovalLevelTypeDto>
    {
        public ApprovalLevelTypeList()
        {
        }
        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public ApprovalLevelTypeList(List<ApprovalLevelTypeDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<ApprovalLevelTypeList> Create(IQueryable<ApprovalLevelType> source, int skip, int pageSize)
        {
            var count = await GetCount(source);
            var dtoList = await GetDtos(source, skip, pageSize);
            var dtoPageList = new ApprovalLevelTypeList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<ApprovalLevelType> source)
        {
            return await source.AsNoTracking().CountAsync();
        }

        public async Task<List<ApprovalLevelTypeDto>> GetDtos(IQueryable<ApprovalLevelType> source, int skip, int pageSize)
        {
            if (pageSize == 0)
            {
                var entities = await source
               .Skip(skip)
               .AsNoTracking()
               .Select(c => new ApprovalLevelTypeDto
               {
                   Id = c.Id,
                   CompanyId = c.CompanyId,
                   TypeName = c.TypeName,
                   Company = c.Company,
               }).ToListAsync();
                return entities;
            }
            else
            {
                var entities = await source
               .Skip(skip)
               .Take(pageSize)
               .AsNoTracking()
               .Select(c => new ApprovalLevelTypeDto
               {
                   Id = c.Id,
                   CompanyId = c.CompanyId,
                   TypeName = c.TypeName,
                   Company = c.Company,
               }).ToListAsync();
                return entities;
            }


        }
    }
}