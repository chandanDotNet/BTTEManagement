using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Dto.Expense
{
    public class ExpenseDocumentDto
    {

        public Guid Id { get; set; }
        public Guid? ExpenseId { get; set; }
        public string ReceiptName { get; set; }
        public string ReceiptPath { get; set; }
        public bool IsReceiptChange { get; set; }
        public IFormFile? FileDetails { get; set; }
    }
}
