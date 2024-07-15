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
    public class BranchList : List<BranchDto>
    {
        public BranchList()
        {

        }

        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public BranchList(List<BranchDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<BranchList> Create(IQueryable<Branch> source, int skip, int pageSize)
        {
            var count = await GetCount(source);
            var dtoList = await GetDtos(source, skip, pageSize);
            var dtoPageList = new BranchList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<Branch> source)
        {
            return await source.AsNoTracking().CountAsync();
        }
        public async Task<List<BranchDto>> GetDtos(IQueryable<Branch> source, int skip, int pageSize)
        {
            if (pageSize == 0)
            {
                var entities = await source
               .Skip(skip)
               .AsNoTracking()
               .Select(c => new BranchDto
               {
                   Id = c.Id,
                   Name = c.Name
               }).ToListAsync();
                return entities;
            }
            else
            {
                var entities = await source
               .Skip(skip)
               //.Take(pageSize)
               .AsNoTracking()
               .Select(c => new BranchDto
               {
                   Id = c.Id,
                   Name = c.Name
               }).ToListAsync();
                return entities;
            }
        }
    }
}