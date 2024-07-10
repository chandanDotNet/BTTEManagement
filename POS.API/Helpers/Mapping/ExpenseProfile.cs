using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.Data.Dto.Expense;
using BTTEM.Data.Entities.Expense;
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
            CreateMap<ExpenseDocument, ExpenseDocumentDto>().ReverseMap();
            CreateMap<MasterExpense, MasterExpenseDto>().ReverseMap();
            CreateMap<GroupExpense, GroupExpenseDto>().ReverseMap();
            CreateMap<AddExpenseCommand, Expense>();
            CreateMap<AddMasterExpenseCommand, MasterExpense>();
            CreateMap<UpdateExpenseCommand, Expense>();
            CreateMap<AddExpenseCategoryCommand, ExpenseCategory>();
            CreateMap<UpdateExpenseCategoryCommand, ExpenseCategory>();
        }
    }
}
