using BTTEM.Data;
using BTTEM.Repository.Expense;
using POS.Common.GenericRepository;
using POS.Data.Resources;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Repository
{
    public interface IMasterExpenseRepository : IGenericRepository<MasterExpense>
    {

        Task<MasterExpenseList> GetAllExpenses(ExpenseResource expenseResource);
    }
}
