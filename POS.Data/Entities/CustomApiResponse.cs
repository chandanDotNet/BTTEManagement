using POS.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Entities
{
    public class CustomApiResponse
    {

        public Guid Id { get; set; }

    }

    public class TripDetailsData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public IList<TripDto> Data { get; set; }
    }
    public class ExpenseCategoryData
    {
        public Guid ExpenseCategoryId { get; set; }
        public string ExpenseCategoryName { get; set; }
        public decimal AllowedAmount { get; set; }
        public decimal ExpenseAmount { get; set; }
        public decimal DeviationAmount { get; set; }
        public List<ExpenseDto> ExpenseDtos { get; set; } = new List<ExpenseDto>();
    }

    public class ExpenseResponseData
    {
        public MasterExpenseDto MaseterExpense { get; set; }
        public Trip Trip { get; set; }
        public IList<ExpenseCategoryData> ExpenseCategories { get; set; } = new List<ExpenseCategoryData>();
    }
}
