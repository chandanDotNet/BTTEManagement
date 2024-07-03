using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.Data.Entities;
using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CommandAndQuery
{
    public class AddLocalConveyanceExpenseCommand : IRequest<ServiceResponse<LocalConveyanceExpenseDto>>
    {

        public Guid Id { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string Particular { get; set; }
        public string Place { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public decimal ApproxKM { get; set; } = 0;
        public string ModeOfTransport { get; set; }
        public decimal Amount { get; set; } = 0;
        public decimal GrandTotal { get; set; } = 0;
        public string Status { get; set; }
        public string Remarks { get; set; }
        public List<LocalConveyanceExpenseDocumentDto> Documents { get; set; }

    }
}
