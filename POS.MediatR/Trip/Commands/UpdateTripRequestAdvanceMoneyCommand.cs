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
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? AdvanceMoneyRemarks { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public DateTime? RequestAdvanceMoneyDate { get; set; }
        public string ProjectType { get; set; }
        public string Remarks { get; set; }
    }
}
