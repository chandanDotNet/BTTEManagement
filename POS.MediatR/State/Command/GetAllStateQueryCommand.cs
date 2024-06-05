using BTTEM.Data.Dto;
using BTTEM.Data.Resources;
using BTTEM.Repository;
using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.State.Command
{
    public class GetAllStateQueryCommand : IRequest<StateList>
    {
        public StateResource StateResource { get; set; }
    }
}
