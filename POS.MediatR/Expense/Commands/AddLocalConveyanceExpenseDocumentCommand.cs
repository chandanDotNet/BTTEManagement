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
    public class AddLocalConveyanceExpenseDocumentCommand : IRequest<ServiceResponse<LocalConveyanceExpenseDocumentDto>>
    {
        public Guid LocalConveyanceExpenseId { get; set; }
        public string ReceiptName { get; set; }
        public string ReceiptPath { get; set; }

    }
}
