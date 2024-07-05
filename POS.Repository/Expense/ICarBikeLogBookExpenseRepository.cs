using BTTEM.Data;
using BTTEM.Data.Resources;
using BTTEM.Repository.Expense;
using POS.Common.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Repository
{
    public interface ICarBikeLogBookExpenseRepository : IGenericRepository<CarBikeLogBookExpense>
    {

        Task<CarBikeLogBookExpenseList> GetAllCarBikeLogBookExpense(CarBikeLogBookExpenseResource expenseResource);
    }
}
