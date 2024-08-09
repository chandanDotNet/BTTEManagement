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
        public bool SGST { get; set; }
        public bool IGST { get; set; }
        public List<ExpenseCategoryTaxDto> ExpenseCategoryTaxes { get; set; }
    }
}
