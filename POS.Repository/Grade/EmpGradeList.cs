using BTTEM.Data;
using Microsoft.EntityFrameworkCore;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Repository
{
    public class EmpGradeList : List<EmpGradeDto>
    {

        private readonly IUserRepository _userRepository;

        public EmpGradeList(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public EmpGradeList(List<EmpGradeDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<EmpGradeList> Create(IQueryable<EmpGrade> source, int skip, int pageSize)
        {
            var count = await GetCount(source);
            var dtoList = await GetDtos(source, skip, pageSize);
            var dtoPageList = new EmpGradeList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<EmpGrade> source)
        {
            return await source.AsNoTracking().CountAsync();
        }

        public async Task<List<EmpGradeDto>> GetDtos(IQueryable<EmpGrade> source, int skip, int pageSize)
        {
            var entities = await source
                .Skip(skip)
                .Take(pageSize)
                .AsNoTracking()
                .Select(c => new EmpGradeDto
                {
                    Id = c.Id,
                    GradeName = c.GradeName,
                    Description = c.Description,
                    IsActive = c.IsActive,
                    NoOfUsers = _userRepository.All.Where(b => b.GradeId == c.Id).Count(),

                }).ToListAsync();
            return entities;
        }
    }
}
