using AutoMapper;
using BTTEM.Data;
using Microsoft.EntityFrameworkCore;
using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Data.Resources;
using POS.Data;
using POS.Domain;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTTEM.Repository.Expense;

namespace BTTEM.Repository
{
    public class MasterExpenseRepository : GenericRepository<MasterExpense, POSDbContext>,
            IMasterExpenseRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IMapper _mapper;
        public MasterExpenseRepository(
            IUnitOfWork<POSDbContext> uow,
            IPropertyMappingService propertyMappingService,
            IMapper mapper
            ) : base(uow)
        {
            _propertyMappingService = propertyMappingService;
            _mapper = mapper;
        }

        public async Task<MasterExpenseList> GetAllExpenses(ExpenseResource expenseResource)
        {
            //var collectionBeforePaging = AllIncluding(c => c.CreatedByUser).ApplySort(expenseResource.OrderBy,
            //    _propertyMappingService.GetPropertyMapping<MasterExpenseDto, MasterExpense>());
            var collectionBeforePaging = AllIncluding(c => c.CreatedByUser,a=>a.Expenses).ApplySort(expenseResource.OrderBy,
                _propertyMappingService.GetPropertyMapping<MasterExpenseDto, MasterExpense>());
            //.ProjectTo<TripDto>(_mapper.ConfigurationProvider).ToListAsync();


            if (expenseResource.TripId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.TripId == expenseResource.TripId);
            }

            if (expenseResource.MasterExpenseId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Id == expenseResource.MasterExpenseId);
            }

            if (expenseResource.ExpenseById.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.CreatedBy == expenseResource.ExpenseById);
            }

            if (expenseResource.FromDate.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Expenses.Any(c=>c.ExpenseDate >= new DateTime(expenseResource.FromDate.Value.Year, expenseResource.FromDate.Value.Month, expenseResource.FromDate.Value.Day, 0, 0, 1)));
            }
            if (expenseResource.ToDate.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Expenses.Any(c=>c.ExpenseDate <= new DateTime(expenseResource.ToDate.Value.Year, expenseResource.ToDate.Value.Month, expenseResource.ToDate.Value.Day, 23, 59, 59)));
            }

            if (expenseResource.ExpenseCategoryId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Expenses.Any(c=>c.ExpenseCategoryId == expenseResource.ExpenseCategoryId));
            }
            if (!string.IsNullOrEmpty(expenseResource.Status))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Status == expenseResource.Status);
            }
            if (!string.IsNullOrEmpty(expenseResource.ApprovalStage))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.ApprovalStage == expenseResource.ApprovalStage);
            }


            return await new MasterExpenseList(_mapper).Create(collectionBeforePaging,
                expenseResource.Skip,
                expenseResource.PageSize);
        }

    }
}
