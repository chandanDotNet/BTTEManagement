using BTTEM.Data;
using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CommandAndQuery
{
    public class AddWalletCommand : IRequest<ServiceResponse<WalletDto>>
    {

       
        public Guid UserId { get; set; }
        public Guid? MasterExpenseId { get; set; }
        public bool IsCredit { get; set; }
        public decimal PermanentAdvance { get; set; }
        public decimal ExpenseAmount { get; set; }
        public decimal CurrentWalletBalance { get; set; }
    }
}
