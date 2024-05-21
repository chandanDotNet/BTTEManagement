using AutoMapper;
using BTTEM.Data.Dto;
using BTTEM.Data.Resources;
using BTTEM.Data;
using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Repository
{
    public class ExpenseTrackingRepository : GenericRepository<ExpenseTracking, POSDbContext>, IExpenseTrackingRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;
        public ExpenseTrackingRepository(IUnitOfWork<POSDbContext> uow,
             IPropertyMappingService propertyMappingService,
             IMapper mapper)
        : base(uow)
        {
            _propertyMappingService = propertyMappingService;
        }


        public async Task<ExpenseTrackingList> GetExpenseTrackings(ExpenseTrackingResource expenseTrackingResource)
        {
            var collectionBeforePaging =
                AllIncluding(r => r.ActionByUser).ApplySort(expenseTrackingResource.OrderBy,
                _propertyMappingService.GetPropertyMapping<ExpenseTrackingDto, ExpenseTracking>());

            if (expenseTrackingResource.MasterExpenseId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                  .Where(a => a.MasterExpenseId == expenseTrackingResource.MasterExpenseId);
            }

            if (expenseTrackingResource.ExpenseId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                  .Where(a => a.ExpenseId == expenseTrackingResource.ExpenseId);
            }

            if (!string.IsNullOrEmpty(expenseTrackingResource.ActionType))
            {
                collectionBeforePaging = collectionBeforePaging
                  .Where(a => a.ActionType == expenseTrackingResource.ActionType);
            }

            var ExpenseTrackingList = new ExpenseTrackingList();
            return await ExpenseTrackingList.Create(collectionBeforePaging, expenseTrackingResource.Skip, expenseTrackingResource.PageSize);
        }
    }
}