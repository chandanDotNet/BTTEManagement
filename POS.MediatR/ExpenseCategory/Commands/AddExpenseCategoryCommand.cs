﻿using POS.Data.Dto;
using POS.Helper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTTEM.Data.Dto;

namespace POS.MediatR.CommandAndQuery
{
    public class AddExpenseCategoryCommand : IRequest<ServiceResponse<ExpenseCategoryDto>>
    {
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
