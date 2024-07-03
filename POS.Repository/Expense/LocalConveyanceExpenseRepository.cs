using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.Data.Resources;
using BTTEM.Repository.Expense;
using Microsoft.EntityFrameworkCore;
using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Data.Resources;
using POS.Domain;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Repository
{
    public class LocalConveyanceExpenseRepository : GenericRepository<LocalConveyanceExpense, POSDbContext>,
            ILocalConveyanceExpenseRepository
    {

        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IMapper _mapper;
        private readonly UserInfoToken _userInfoToken;
        private readonly IUserRoleRepository _userRoleRepository;
        public LocalConveyanceExpenseRepository(
            IUnitOfWork<POSDbContext> uow,
            IPropertyMappingService propertyMappingService,
            IMapper mapper,
             UserInfoToken userInfoToken,
              IUserRoleRepository userRoleRepository
            ) : base(uow)
        {
            _propertyMappingService = propertyMappingService;
            _mapper = mapper;
            _userInfoToken = userInfoToken;
            _userRoleRepository = userRoleRepository;
        }

        public async Task<LocalConveyanceExpenseList> GetAllLocalConveyanceExpense(LocalConveyanceExpenseResource expenseResource)
        {

           // Guid LoginUserId = Guid.Parse(_userInfoToken.Id);
            // var Role = GetUserRole(LoginUserId).Result.FirstOrDefault();

            var collectionBeforePaging = All.Include(t => t.CreatedByUser).Include(a=>a.Documents).ApplySort(expenseResource.OrderBy,
              _propertyMappingService.GetPropertyMapping<LocalConveyanceExpenseDto, LocalConveyanceExpense>());

            if (expenseResource.CreatedBy.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.CreatedBy == expenseResource.CreatedBy);
            }

            return await new LocalConveyanceExpenseList(_mapper).Create(collectionBeforePaging,
              expenseResource.Skip,
              expenseResource.PageSize);

        }

    }
}
