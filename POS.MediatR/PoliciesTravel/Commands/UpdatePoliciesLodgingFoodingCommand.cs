using BTTEM.Data;
using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.PoliciesTravel.Commands
{
    public class UpdatePoliciesLodgingFoodingCommand : IRequest<ServiceResponse<bool>>
    {
         public Guid Id { get; set; }
        public Guid PoliciesDetailId { get; set; }
        public bool IsMetroCities { get; set; }
        public decimal MetroCitiesUptoAmount { get; set; }
        public bool OtherCities { get; set; }
        public decimal OtherCitiesUptoAmount { get; set; }
        public bool IsFoodActuals { get; set; }
        public bool IsBudget { get; set; }
        public decimal BudgetAmount { get; set; }
        public bool IsDeleted { get; set; }

    }
}
