using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
using Microsoft.EntityFrameworkCore;
using POS.Data;
using POS.Data.Dto;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BTTEM.Repository.Expense
{
    public class MasterExpenseList : List<MasterExpenseDto>
    {
        IMapper _mapper;
        IUserRepository _userRepository;
        public MasterExpenseList(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public decimal TotalAmount { get; set; }
        public string Filter { get; set; }
        public MasterExpenseList(List<MasterExpenseDto> items, int count, int skip, int pageSize, decimal totalAmount)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalAmount = totalAmount;
            AddRange(items);
        }

        public async Task<MasterExpenseList> Create(IQueryable<MasterExpense> source, int skip, int pageSize, string filter)
        {

            var dtoList = await GetDtos(source, skip, pageSize, filter);
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

        public async Task<List<MasterExpenseDto>> GetDtos(IQueryable<MasterExpense> source, int skip, int pageSize, string filter)
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
                        FirstLevelReimbursementAmount = cs.FirstLevelReimbursementAmount,
                        SecondLevelReimbursementAmount = cs.SecondLevelReimbursementAmount,
                        ThirdLevelReimbursementAmount = cs.ThirdLevelReimbursementAmount,
                        AdvanceMoney = cs.AdvanceMoney,
                        ApprovalStage = cs.ApprovalStage,
                        ApprovalStageBy = cs.ApprovalStageBy,
                        ApprovalStageDate = cs.ApprovalStageDate,
                        NoOfBill = cs.NoOfBill,
                        //ExpenseByUser= cs.ExpenseByUser,
                        //NoOfBill = cs.Expenses.Count,
                        ExpenseByUser = cs.ExpenseByUser,
                        Status = cs.Status,
                        ExpenseType = cs.ExpenseType,
                        CreatedDate = cs.CreatedDate,
                        ReimbursementStatus = cs.ReimbursementStatus,
                        IsExpenseCompleted = cs.IsExpenseCompleted,
                        RollbackCount = cs.RollbackCount != null ? cs.RollbackCount : 0,
                        CreatedByUser = cs.CreatedByUser != null ? _mapper.Map<UserDto>(cs.CreatedByUser) : null,
                        Expenses = _mapper.Map<List<ExpenseDto>>(cs.Expenses).ToList(),
                        //TotalDeviation = _mapper.Map<List<ExpenseDto>>(cs.Expenses).ToList().Sum(x => x.DeviationAmount),
                        Trip = cs.Trip,
                        JourneyNumber = cs.JourneyNumber,
                        ReimbursementRemarks = cs.ReimbursementRemarks,
                        IsGroupExpense = cs.IsGroupExpense,
                        NoOfPerson = cs.NoOfPerson,
                        GroupExpenses = _mapper.Map<List<GroupExpenseDto>>(cs.GroupExpenses),
                        IsGroupTrip = cs.Trip.IsGroupTrip.Value,
                        GroupTrips = _mapper.Map<List<GroupTripDto>>(cs.Trip.GroupTrips),
                        //CompanyAccount = _mapper.Map<CompanyAccountDto>(cs.CreatedByUser.CompanyAccounts),
                        IsExpenseChecker = cs.IsExpenseChecker,
                        AccountsApprovalStage = cs.AccountsApprovalStage,
                        AccountsCheckerOneId = cs.AccountsCheckerOneId,
                        LevelOneUser = string.Concat(cs.LevelOneUser.FirstName, ' ', cs.LevelOneUser.LastName),
                        AccountsCheckerOneStatus = cs.AccountsCheckerOneStatus,
                        AccountsCheckerTwoId = cs.AccountsCheckerTwoId,
                        LevelTwoUser = string.Concat(cs.LevelTwoUser.FirstName, ' ', cs.LevelTwoUser.LastName),
                        AccountsCheckerTwoStatus = cs.AccountsCheckerTwoStatus,
                        AccountsCheckerThreeId = cs.AccountsCheckerThreeId,
                        LevelThreeUser = string.Concat(cs.LevelThreeUser.FirstName, ' ', cs.LevelThreeUser.LastName),
                        AccountsCheckerThreeStatus = cs.AccountsCheckerThreeStatus,
                        CompanyAccountId = cs.CompanyAccountId,
                        BillingCompanyAccount = _mapper.Map<CompanyAccountDto>(cs.CompanyAccounts),
                        ReceiptPath = cs.ReceiptPath,
                        ReceiptName = cs.ReceiptName,
                        AccountTeam = cs.AccountTeam,
                        SapJourneyNumber = cs.SapJourneyNumber,
                        SapDocumentNumber = cs.SapDocumentNumber,
                        ExpensesAppliedOn=cs.ExpensesAppliedOn,

                        //GroupTrips = cs.Trip.GroupTrips.Select(c => new GroupTripDto
                        //{
                        //    Id = c.Id,
                        //    UserId = c.UserId,
                        //    TripId = c.TripId,
                        //    //User = _mapper.Map<User>(_userRepository.FindAsync(c.UserId))

                        //}).ToList(),

                        //_mapper.Map<List<GroupTripDto>>(cs.Trip.GroupTrips),

                    })//.OrderByDescending(x => x.CreatedDate) 
                    .ToListAsync();

                foreach (var item in entities)
                {
                    if (item.TripId != null)
                    {
                        foreach (var data in item.GroupTrips)
                        {
                            data.User = await _userRepository.FindAsync(data.UserId);
                        }
                    }
                    item.TotalDeviation = item.Expenses.ToList().Sum(x => x.DeviationAmount);
                    item.Remarks = item.Expenses.ToList().FirstOrDefault().Description;
                }

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
                   FirstLevelReimbursementAmount = cs.FirstLevelReimbursementAmount,
                   SecondLevelReimbursementAmount = cs.SecondLevelReimbursementAmount,
                   ThirdLevelReimbursementAmount = cs.ThirdLevelReimbursementAmount,
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
                   //TotalDeviation = _mapper.Map<List<ExpenseDto>>(cs.Expenses).ToList().Sum(x => x.DeviationAmount),
                   Trip = cs.Trip,
                   JourneyNumber = cs.JourneyNumber,
                   ReimbursementRemarks = cs.ReimbursementRemarks,
                   IsGroupExpense = cs.IsGroupExpense,
                   NoOfPerson = cs.NoOfPerson,
                   GroupExpenses = _mapper.Map<List<GroupExpenseDto>>(cs.GroupExpenses),
                   IsGroupTrip = cs.Trip.IsGroupTrip.Value,
                   GroupTrips = _mapper.Map<List<GroupTripDto>>(cs.Trip.GroupTrips),
                   //CompanyAccount = _mapper.Map<CompanyAccountDto>(cs.CreatedByUser.CompanyAccounts),
                   IsExpenseChecker = cs.IsExpenseChecker,
                   AccountsApprovalStage = cs.AccountsApprovalStage,
                   AccountsCheckerOneId = cs.AccountsCheckerOneId,
                   LevelOneUser = string.Concat(cs.LevelOneUser.FirstName, ' ', cs.LevelOneUser.LastName),
                   AccountsCheckerOneStatus = cs.AccountsCheckerOneStatus,
                   AccountsCheckerTwoId = cs.AccountsCheckerTwoId,
                   LevelTwoUser = string.Concat(cs.LevelTwoUser.FirstName, ' ', cs.LevelTwoUser.LastName),
                   AccountsCheckerTwoStatus = cs.AccountsCheckerTwoStatus,
                   AccountsCheckerThreeId = cs.AccountsCheckerThreeId,
                   LevelThreeUser = string.Concat(cs.LevelThreeUser.FirstName, ' ', cs.LevelThreeUser.LastName),
                   AccountsCheckerThreeStatus = cs.AccountsCheckerThreeStatus,
                   CompanyAccountId = cs.CompanyAccountId,
                   BillingCompanyAccount = _mapper.Map<CompanyAccountDto>(cs.CompanyAccounts),
                   ReceiptPath = cs.ReceiptPath,
                   ReceiptName = cs.ReceiptName,
                   AccountTeam = cs.AccountTeam,
                   SapJourneyNumber = cs.SapJourneyNumber.Value,
                   SapDocumentNumber = cs.SapDocumentNumber,
                   ExpensesAppliedOn = cs.ExpensesAppliedOn,
                   //GroupTrips = cs.Trip.GroupTrips.Select(c => new GroupTripDto
                   //{
                   //    Id = c.Id,
                   //    UserId = c.UserId,
                   //    TripId = c.TripId,                     
                   //    //User = _mapper.Map<User>(_userRepository.FindAsync(c.UserId))

                   //}).ToList(),
               })//.OrderByDescending(x => x.CreatedDate)
               .ToListAsync();

                foreach (var item in entities)
                {
                    if (item.TripId != null)
                    {
                        foreach (var data in item.GroupTrips)
                        {
                            data.User = await _userRepository.FindAsync(data.UserId);
                        }
                    }
                    item.TotalDeviation = item.Expenses.ToList().Sum(x => x.DeviationAmount);
                    item.Remarks = item.Expenses.ToList().FirstOrDefault().Description;
                }

                if (!string.IsNullOrEmpty(filter))
                {
                    var _lstExpense = entities.Select(s =>
                                         s.Expenses.Where(x => x.Status == filter)).ToList();

                    IEnumerable<ExpenseDto> flattened = _lstExpense.SelectMany(list => list);

                    entities.ForEach(item =>
                    {
                        item.Expenses = flattened.ToList();
                    });
                }

                return entities;
            }
        }

    }
}
