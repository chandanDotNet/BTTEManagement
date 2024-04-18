using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
using POS.Data;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;

namespace POS.API.Helpers.Mapping
{
    public class ExpenseProfile : Profile
    {
        public ExpenseProfile()
        {
            CreateMap<ExpenseCategory, ExpenseCategoryDto>().ReverseMap();
            CreateMap<Expense, ExpenseDto>().ReverseMap();
            CreateMap<MasterExpense, MasterExpenseDto>().ReverseMap();
            CreateMap<AddExpenseCommand, Expense>();
            CreateMap<AddMasterExpenseCommand, MasterExpense>();
            CreateMap<UpdateExpenseCommand, Expense>();
            CreateMap<AddExpenseCategoryCommand, ExpenseCategory>();
            CreateMap<UpdateExpenseCategoryCommand, ExpenseCategory>();
        }
    }
}
