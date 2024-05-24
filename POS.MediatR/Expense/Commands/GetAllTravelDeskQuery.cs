using BTTEM.Data.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Expense.Commands
{
    public class GetAllTravelDeskQuery : IRequest<List<TravelDeskExpenseDto>>
    {

        public Guid? Id { get; set; }
    }
}
