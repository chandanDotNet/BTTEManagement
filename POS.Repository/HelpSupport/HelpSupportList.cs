using BTTEM.Data;
using BTTEM.Data.Dto;
using Microsoft.EntityFrameworkCore;
using POS.Common.GenericRepository;
using POS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Repository
{
    public class HelpSupportList : List<HelpSupportDto>
    {
        public HelpSupportList()
        {
        }
        public int Skip { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public HelpSupportList(List<HelpSupportDto> items, int count, int skip, int pageSize)
        {
            Skip = skip;
            TotalPages = pageSize;
            PageSize = pageSize;
            TotalCount = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }
        public async Task<HelpSupportList> Create(IQueryable<HelpSupport> source, int skip, int pageSize)
        {
            var count = await GetCount(source);
            var dtoList = await GetDtos(source, skip, pageSize);
            var dtoPageList = new HelpSupportList(dtoList, count, skip, pageSize);
            return dtoPageList;

        }
        public async Task<int> GetCount(IQueryable<HelpSupport> source)
        {
            return await source.AsNoTracking().CountAsync();
        }
        public async Task<List<HelpSupportDto>> GetDtos(IQueryable<HelpSupport> source, int skip, int pageSize)
        {
            var entities =
                await source
                .Skip(skip)
                .Take(pageSize)
                .AsNoTracking()
                .Select(c => new HelpSupportDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Question = c.Question,
                    Answer = c.Answer,
                    Description = c.Description,
                }).ToListAsync();
            return entities;
        }
    }
}
