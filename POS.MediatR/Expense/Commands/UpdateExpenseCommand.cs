﻿using POS.Helper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTTEM.Data.Dto.Expense;
using BTTEM.Data.Dto;

namespace POS.MediatR.CommandAndQuery
{
    public class UpdateExpenseCommand : IRequest<ServiceResponse<bool>>
    {
        public Guid Id { get; set; }
        public Guid? MasterExpenseId { get; set; }
        public Guid? TripId { get; set; }
        public string Name { get; set; }
        public string BillType { get; set; }
        public string GSTNo { get; set; }
        public string Reference { get; set; }
        public Guid ExpenseCategoryId { get; set; }
        public decimal Amount { get; set; }
        public Guid? ExpenseById { get; set; }
        public string Description { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string ReceiptName { get; set; }
        public bool IsReceiptChange { get; set; }
        public string DocumentData { get; set; }
        public string Status { get; set; }
        public string ReUploadRemarks { get; set; }
        public List<ExpenseDocumentDto> ExpenseDocument { get; set; }
        public List<ExpenseDetailDto> ExpenseDetail { get; set; }

    }
}
