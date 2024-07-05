using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Commands
{
    public class DeleteLocalConveyanceExpenseCommand : IRequest<ServiceResponse<bool>>
    {

        public Guid Id { get; set; }
    }
}
