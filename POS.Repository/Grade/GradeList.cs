using BTTEM.Data;
using Microsoft.EntityFrameworkCore;
using POS.Data;
using POS.Data.Dto;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Repository
{
    public class GradeList : List<GradeDto>
    {
        private readonly IUserRepository _userRepository;

        public GradeList(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public GradeList(List<GradeDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<GradeList> Create(IQueryable<Grade> source, int skip, int pageSize)
        {
            var count = await GetCount(source);
            var dtoList = await GetDtos(source, skip, pageSize);
            var dtoPageList = new GradeList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<Grade> source)
        {
            return await source.AsNoTracking().CountAsync();
        }

        public async Task<List<GradeDto>> GetDtos(IQueryable<Grade> source, int skip, int pageSize)
        {
            var entities = await source
                .Skip(skip)
                .Take(pageSize)
                .AsNoTracking()
                .Select(c => new GradeDto
                {
                    Id = c.Id,
                    GradeName = c.GradeName,
                    Description = c.Description,
                    IsActive=c.IsActive,
                    NoOfUsers = _userRepository.All.Where(b => b.GradeId == c.Id).Count(),
                   
                }).ToListAsync();
            return entities;
        }

    }
}
