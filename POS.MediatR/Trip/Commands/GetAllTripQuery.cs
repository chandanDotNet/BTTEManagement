using BTTEM.Data;
using BTTEM.Data.Resources;
using BTTEM.Repository;
using BTTEM.Repository.Expense;
using MediatR;
using POS.Data.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CommandAndQuery
{
    public class GetAllTripQuery : IRequest<TripList>
    {
        public TripResource TripResource { get; set; }
    }
}
