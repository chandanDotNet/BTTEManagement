using BTTEM.Data.Resources;
using BTTEM.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CommandAndQuery
{
    public class GetAllAdvanceMoneyQuery : IRequest<AdvanceMoneyList>
    {
        public AdvanceMoneyResource AdvanceMoneyResource { get; set; }
    }
}

