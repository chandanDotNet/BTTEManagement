using AutoMapper;
using Microsoft.EntityFrameworkCore;
using POS.Data.Dto;
using POS.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTTEM.Data;
using POS.Helper;

namespace BTTEM.Repository
{
    

    public class PoliciesDetailList : List<PoliciesDetailDto>
    {
        public IMapper _mapper { get; set; }
        public PathHelper _pathHelper { get; set; }
        public PoliciesDetailList(IMapper mapper, PathHelper pathHelper)
        {
            _mapper = mapper;
            _pathHelper = pathHelper;
        }

        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public PoliciesDetailList(List<PoliciesDetailDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<PoliciesDetailList> Create(IQueryable<PoliciesDetail> source, int skip, int pageSize)
        {
            var count = await GetCount(source);
            var dtoList = await GetDtos(source, skip, pageSize);
            var dtoPageList = new PoliciesDetailList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<PoliciesDetail> source)
        {
            return await source.AsNoTracking().CountAsync();
        }

        public async Task<List<PoliciesDetailDto>> GetDtos(IQueryable<PoliciesDetail> source, int skip, int pageSize)
        {
            var entities = await source
                .Skip(skip)
                .Take(pageSize)
                .AsNoTracking()
                .Select(c => new PoliciesDetailDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    GradeId= c.GradeId,
                    GradeName=c.Grade.GradeName,
                    Description = c.Description,
                    DailyAllowance = c.DailyAllowance,
                    Document= c.Document,
                    IsActive= c.IsActive,
                   
                }).ToListAsync();
            return entities;
        }
    }
}
