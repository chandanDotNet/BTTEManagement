using BTTEM.Data.Dto;
using BTTEM.MediatR.CommandAndQuery;
using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Expense.Commands
{
    public class AddCarBikeLogBookExpenseCommandForApp : IRequest<ServiceResponse<CarBikeLogBookExpenseDto>>
    {
        public List<AddCarBikeLogBookExpenseCommand> addCarBikeLogBookExpenseCommandList { get; set; }
    }
}
