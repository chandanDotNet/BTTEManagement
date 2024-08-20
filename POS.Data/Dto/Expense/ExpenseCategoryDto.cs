using BTTEM.Data.Dto;
using System;
using System.Collections.Generic;

namespace POS.Data.Dto
{
    public class ExpenseCategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }
        public string SSECode { get; set; }
        public List<ExpenseCategoryTaxDto> ExpenseCategoryTaxes { get; set; }
    }
}
