﻿using BTTEM.Data.Dto;
using BTTEM.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Repository
{
    public class ExpenseTrackingList : List<ExpenseTrackingDto>
    {
        public ExpenseTrackingList()
        {
        }
        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public ExpenseTrackingList(List<ExpenseTrackingDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<ExpenseTrackingList> Create(IQueryable<ExpenseTracking> source, int skip, int pageSize)
        {
            var count = await GetCount(source);
            var dtoList = await GetDtos(source, skip, pageSize);
            var dtoPageList = new ExpenseTrackingList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<ExpenseTracking> source)
        {
            return await source.AsNoTracking().CountAsync();
        }

        public async Task<List<ExpenseTrackingDto>> GetDtos(IQueryable<ExpenseTracking> source, int skip, int pageSize)
        {
            var entities = await source
                .Skip(skip)
                .Take(pageSize)
                .AsNoTracking()
                .Select(c => new ExpenseTrackingDto
                {
                    Id = c.Id,
                    MasterExpenseId = c.MasterExpenseId,
                    ExpenseId = c.ExpenseId,
                    ActionType = c.ActionType,
                    Status = c.Status,
                    Remarks = c.Remarks,
                    ActionBy = c.ActionBy,
                    ActionByName = c.ActionByUser.FirstName + " " + c.ActionByUser.LastName,
                    ActionDate = c.ActionDate,
                    ExpenseTypeName = c.ExpenseTypeName,

                }).ToListAsync();
            return entities;
        }
    }
}