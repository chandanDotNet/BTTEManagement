using POS.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Dto
{
    public class ExpenseCategoryTaxDto
    {
        public Guid? ExpenseCategoryId { get; set; }
        public Guid TaxId { get; set; }
        public TaxDto Tax { get; set; }
   
    }
}
