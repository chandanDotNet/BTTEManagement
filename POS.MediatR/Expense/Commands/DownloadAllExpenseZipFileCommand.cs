using BTTEM.Data.Dto.Expense;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Expense.Commands
{
    public class DownloadAllExpenseZipFileCommand : IRequest<bool>
    {
        public Guid MasterExpenseId { get; set; }
    }
}
