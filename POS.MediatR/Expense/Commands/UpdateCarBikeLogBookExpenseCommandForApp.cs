using BTTEM.Data.Dto;
using BTTEM.MediatR.Commands;
using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Expense.Commands
{
    public class UpdateCarBikeLogBookExpenseCommandForApp : IRequest<ServiceResponse<bool>>
    {
        public List<UpdateCarBikeLogBookExpenseCommand> updateCarBikeLogBookExpenseCommandList { get; set; }

    }
}
