using BTTEM.Data;
using BTTEM.Data.Dto;
using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CommandAndQuery
{
    public class AddExpenseDetailCommand : IRequest<ServiceResponse<ExpenseDetailDto>>
    {

        public Guid ExpenseId { get; set; }
        public Guid? VendorId { get; set; }
        public string? VendorName { get; set; }
        public Guid? SubVendorId { get; set; }
        public string? SubVendorName { get; set; }
        public decimal? BasicValue { get; set; }
        public decimal? TaxAmount { get; set; }
        public string? TaxCode { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? TaxInvoiceNumber { get; set; }
        public bool? IsIRN { get; set; }
        public bool? IsTaxable { get; set; }
    }
}
