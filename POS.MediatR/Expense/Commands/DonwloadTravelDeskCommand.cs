using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CommandAndQuery
{
    public class DonwloadTravelDeskCommand : IRequest<string>
    {
        public Guid Id { get; set; }
    }
}
