using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Trip.Commands
{
    public class UpdateTripRequestAdvanceMoneyCommand : IRequest<ServiceResponse<bool>>
    {

        public Guid Id { get; set; }
        public bool IsRequestAdvanceMoney { get; set; }
        public decimal? AdvanceMoney { get; set; }
        public string? AdvanceMoneyRemarks { get; set; }
    }
}
