using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Trip.Commands
{
    public class UpdateStatusTripRequestAdvanceMoneyCommand : IRequest<ServiceResponse<bool>>
    {

        public Guid Id { get; set; }
        public Guid StatusUpdatedBy { get; set; }
        public string Status { get; set; }
        public DateTime RequestAdvanceMoneyDate { get; set; }

    }
}
