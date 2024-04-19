using BTTEM.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CommandAndQuery
{
    public class GetAllTripQuery : IRequest<List<TripDto>>
    {
        public Guid? Id { get; set; }
        public Guid? CreatedBy { get; set; }
    }
}
