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
    public class CompanyAccountList : List<CompanyAccountDto>
    {
        public CompanyAccountList()
        {
        }
        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public CompanyAccountList(List<CompanyAccountDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<CompanyAccountList> Create(IQueryable<CompanyAccount> source, int skip, int pageSize)
        {
            var count = await GetCount(source);
            var dtoList = await GetDtos(source, skip, pageSize);
            var dtoPageList = new CompanyAccountList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<CompanyAccount> source)
        {
            return await source.AsNoTracking().CountAsync();
        }

        public async Task<List<CompanyAccountDto>> GetDtos(IQueryable<CompanyAccount> source, int skip, int pageSize)
        {
            var entities = await source
                .Skip(skip)
                .Take(pageSize)
                .Select(c => new CompanyAccountDto
                {
                    Id = c.Id,
                    AccountName = c.AccountName,
                    CompanyProfileId = c.CompanyProfileId,
                    GSTCount = c.CompanyGST.Count(),
                    ReceiptName = c.ReceiptName,
                    AccountCode = c.AccountCode,
                    ReceiptPath = c.ReceiptPath,
                    CompanyAccountsApprovalLevel = c.CompanyAccountsApprovalLevel,
                    AccountTeam = c.AccountTeam,

                }).ToListAsync();
            return entities;
        }
    }
}