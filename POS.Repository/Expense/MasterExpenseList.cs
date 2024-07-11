using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
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
                        PayableAmount = cs.PayableAmount,
                        AdvanceMoney = cs.AdvanceMoney,
                        ApprovalStage = cs.ApprovalStage,                        
                        ApprovalStageBy = cs.ApprovalStageBy,                        
                        ApprovalStageDate = cs.ApprovalStageDate,                        
                        NoOfBill = cs.NoOfBill,
                        //ExpenseByUser= cs.ExpenseByUser,
                        //NoOfBill = cs.Expenses.Count,
                        ExpenseByUser = cs.ExpenseByUser,
                        Status = cs.Status,
                        ExpenseType= cs.ExpenseType,
                        CreatedDate = cs.CreatedDate,
                        ReimbursementStatus = cs.ReimbursementStatus,
                        IsExpenseCompleted = cs.IsExpenseCompleted,
                        RollbackCount = cs.RollbackCount != null ? cs.RollbackCount : 0,
                        CreatedByUser = cs.CreatedByUser != null ? _mapper.Map<UserDto>(cs.CreatedByUser) : null,
                        Expenses =  _mapper.Map<List<ExpenseDto>>(cs.Expenses).ToList(),
                        Trip = cs.Trip,
                        JourneyNumber = cs.JourneyNumber,
                        ReimbursementRemarks = cs.ReimbursementRemarks,
                        IsGroupExpense= cs.IsGroupExpense,
                        NoOfPerson = cs.NoOfPerson,
                        GroupExpenses = _mapper.Map<List<GroupExpenseDto>>(cs.GroupExpenses),

                    })//.OrderByDescending(x => x.CreatedDate) 
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
                 PayableAmount = cs.PayableAmount,
                 ReimbursementAmount = cs.ReimbursementAmount,
                 AdvanceMoney = cs.AdvanceMoney,
                 ApprovalStage = cs.ApprovalStage,
                 ApprovalStageBy = cs.ApprovalStageBy,
                 ApprovalStageDate = cs.ApprovalStageDate,
                 NoOfBill = cs.NoOfBill,
                 //ExpenseByUser= cs.ExpenseByUser,                 
                 //NoOfBill = cs.Expenses.Count,
                 Status = cs.Status,
                 ExpenseType = cs.ExpenseType,
                 CreatedDate = cs.CreatedDate,
                 ReimbursementStatus = cs.ReimbursementStatus,
                 IsExpenseCompleted = cs.IsExpenseCompleted,
                 RollbackCount = cs.RollbackCount != null ? cs.RollbackCount : 0,
                 CreatedByUser = cs.CreatedByUser != null ? _mapper.Map<UserDto>(cs.CreatedByUser) : null,
                 Expenses = _mapper.Map<List<ExpenseDto>>(cs.Expenses).ToList(),
                 Trip = cs.Trip,
                 JourneyNumber = cs.JourneyNumber,
                 ReimbursementRemarks = cs.ReimbursementRemarks,
                 IsGroupExpense = cs.IsGroupExpense,
                 NoOfPerson = cs.NoOfPerson,
                 GroupExpenses = _mapper.Map<List<GroupExpenseDto>>(cs.GroupExpenses),
             })//.OrderByDescending(x => x.CreatedDate)
             .ToListAsync();
                return entities;
            }
        }

    }
}
