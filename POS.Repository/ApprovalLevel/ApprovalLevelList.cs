using BTTEM.Data.Dto;
using BTTEM.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using POS.Data.Dto;

namespace BTTEM.Repository
{
    public class ApprovalLevelList : List<ApprovalLevelDto>
    {
        IMapper _mapper;
        public ApprovalLevelList(IMapper mapper)
        {
            _mapper = mapper;
        }
        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public ApprovalLevelList(List<ApprovalLevelDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<ApprovalLevelList> Create(IQueryable<ApprovalLevel> source, int skip, int pageSize)
        {
            var count = await GetCount(source);
            var dtoList = await GetDtos(source, skip, pageSize);
            var dtoPageList = new ApprovalLevelList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<ApprovalLevel> source)
        {
            return await source.AsNoTracking().CountAsync();
        }

        public async Task<List<ApprovalLevelDto>> GetDtos(IQueryable<ApprovalLevel> source, int skip, int pageSize)
        {
            if (pageSize == 0)
            {
                var entities = await source
               .Skip(skip)
               .AsNoTracking()
               .Select(c => new ApprovalLevelDto
               {
                   Id = c.Id,
                   ApprovalLevelTypeId = c.ApprovalLevelTypeId,
                   ApprovalLevelType = _mapper.Map<ApprovalLevelTypeDto>(c.ApprovalLevelType),
                   LevelName = c.LevelName,
                   OrderNo = c.OrderNo,
                   RoleId = c.RoleId,
                   Role = _mapper.Map<RoleDto>(c.Role)

               }).ToListAsync();
                return entities;
            }
            else
            {
                var entities = await source
               .Skip(skip)
               .Take(pageSize)
               .AsNoTracking()
               .Select(c => new ApprovalLevelDto
               {
                   Id = c.Id,
                   ApprovalLevelTypeId = c.ApprovalLevelTypeId,
                   ApprovalLevelType = _mapper.Map<ApprovalLevelTypeDto>(c.ApprovalLevelType),
                   LevelName = c.LevelName,
                   OrderNo = c.OrderNo,
                   RoleId = c.RoleId,
                   Role = _mapper.Map<RoleDto>(c.Role)

               }).ToListAsync();
                return entities;
            }
        }
    }
}