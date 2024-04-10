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
    public class AddConveyanceCommand : IRequest<ServiceResponse<ConveyanceDto>>
    {

        public List<ConveyanceDto> conveyancesData { get; set; }
        // public Guid Id { get; set; }
        //public string Name { get; set; }
        //public bool IsMaster { get; set; }
        //public Guid? PoliciesDetailId { get; set; }
        //public bool IsDeleted { get; set; }


        //public List<ConveyancesItemDto> conveyancesItem { get; set; }
    }
}
