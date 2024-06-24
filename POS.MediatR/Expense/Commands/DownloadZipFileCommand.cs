using BTTEM.Data.Dto.Expense;
using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Expense.Commands
{
    public class DownloadZipFileCommand :IRequest<List<ExpenseDocumentDto>>
    {
        public Guid ExpenseId { get; set; }
    }
}
