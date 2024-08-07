﻿using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.Data.Dto.Expense;
using BTTEM.Data.Entities.Expense;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Commands;
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
            CreateMap<ExpenseCategoryTax, ExpenseCategoryTaxDto>().ReverseMap();
            CreateMap<Expense, ExpenseDto>().ReverseMap();
            CreateMap<ExpenseDocument, ExpenseDocumentDto>().ReverseMap();
            CreateMap<ExpenseDetail, ExpenseDetailDto>().ReverseMap();
            CreateMap<MasterExpense, MasterExpenseDto>().ReverseMap();
            CreateMap<GroupExpense, GroupExpenseDto>().ReverseMap();
            CreateMap<AddExpenseCommand, Expense>();
            CreateMap<AddMasterExpenseCommand, MasterExpense>();
            CreateMap<UpdateExpenseCommand, Expense>();
            CreateMap<AddExpenseCategoryCommand, ExpenseCategory>();
            CreateMap<UpdateExpenseCategoryCommand, ExpenseCategory>();
            CreateMap<AddExpenseDetailCommand, ExpenseDetail>();
            CreateMap<UpdateExpenseDetailCommand, ExpenseDetail>();
        }
    }
}
