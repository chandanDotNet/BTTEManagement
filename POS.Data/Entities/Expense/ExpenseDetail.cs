﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data
{
    public class ExpenseDetail 
    {
        public Guid Id { get; set; }
        public Guid ExpenseId { get; set; }
        public Guid? MasterExpenseId { get; set; }
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
        public string? GSTType { get; set; }
        public decimal? CGST { get; set; }
        public decimal? SGST { get; set; }
        public decimal? IGST { get; set; }
        public string SSECode { get; set; }
        public string BillCreditType { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? PostingDate { get; set; }
        public string CostCenter { get; set; }
    }
}
