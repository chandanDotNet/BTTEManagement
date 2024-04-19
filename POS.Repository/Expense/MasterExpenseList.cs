using AutoMapper;
using BTTEM.Data;
using Microsoft.EntityFrameworkCore;
using POS.Data.Dto;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Repository.Expense
{
    public class MasterExpenseList : List<MasterExpenseDto>
    {

        IMapper _mapper;
        public MasterExpenseList(IMapper mapper)
        {
            _mapper = mapper;
        }

        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public decimal TotalAmount { get; set; }

        public MasterExpenseList(List<MasterExpenseDto> items, int count, int skip, int pageSize, decimal totalAmount)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalAmount = totalAmount;
            AddRange(items);
        }

        public async Task<MasterExpenseList> Create(IQueryable<MasterExpense> source, int skip, int pageSize)
        {

            var dtoList = await GetDtos(source, skip, pageSize);
            var count = pageSize == 0 || dtoList.Count() == 0 ? dtoList.Count() : await GetCount(source);
            var totalAmount = await GetTotalAmount(source);
            var dtoPageList = new MasterExpenseList(dtoList, count, skip, pageSize, totalAmount);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<MasterExpense> source)
        {
            try
            {
                return await source.AsNoTracking().CountAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<decimal> GetTotalAmount(IQueryable<MasterExpense> source)
        {
            try
            {
                return await source.AsNoTracking().SumAsync(c => c.TotalAmount);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<MasterExpenseDto>> GetDtos(IQueryable<MasterExpense> source, int skip, int pageSize)
        {
            if (pageSize == 0)
            {
                var entities = await source
                    .AsNoTracking()
                    .Select(cs => new MasterExpenseDto
                    {
                        Id = cs.Id,
                        ExpenseNo = cs.ExpenseNo,
                        TripId = cs.TripId,
                        Name = cs.Name,
                        TotalAmount = cs.TotalAmount,
                        ReimbursementAmount = cs.ReimbursementAmount,
                        ApprovalStage = cs.ApprovalStage,
                        NoOfBill = cs.NoOfBill,
                        Status = cs.Status,
                        ExpenseType= cs.ExpenseType,
                        CreatedDate = cs.CreatedDate,
                        CreatedByUser = cs.CreatedByUser != null ? _mapper.Map<UserDto>(cs.CreatedByUser) : null,
                        Expenses =  _mapper.Map<List<ExpenseDto>>(cs.Expenses).ToList(),
                    })
                    .ToListAsync();
                return entities;
            }
            else
            {
                var entities = await source
             .Skip(skip)
             .Take(pageSize)
             .AsNoTracking()
             .Select(cs => new MasterExpenseDto
             {
                 Id = cs.Id,
                 ExpenseNo = cs.ExpenseNo,
                 TripId = cs.TripId,
                 Name = cs.Name,
                 TotalAmount = cs.TotalAmount,
                 ReimbursementAmount = cs.ReimbursementAmount,
                 ApprovalStage = cs.ApprovalStage,
                 NoOfBill = cs.NoOfBill,
                 Status = cs.Status,
                 ExpenseType = cs.ExpenseType,
                 CreatedDate = cs.CreatedDate,
                 CreatedByUser = cs.CreatedByUser != null ? _mapper.Map<UserDto>(cs.CreatedByUser) : null,
                 Expenses = _mapper.Map<List<ExpenseDto>>(cs.Expenses).ToList(),
             })
             .ToListAsync();
                return entities;
            }
        }

    }
}
