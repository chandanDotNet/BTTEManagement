using BTTEM.Data;
using BTTEM.Data.Dto.PoliciesTravel;
using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CommandAndQuery
{
    public class AddTravelModeCommand : IRequest<ServiceResponse<TravelModeDto>>
    {

        public string TravalModesType { get; set; }
        public List<TravelModeDto> TravelModeData { get; set; }
        // public Guid Id { get; set; }
        //public string TravelsModesName { get; set; }
        //public string TravelsModesImage { get; set; }
        //public Guid? PoliciesDetailId { get; set; }
        //public bool IsMaster { get; set; }
        //public bool IsDeleted { get; set; }
        //public List<ClassOfTravelDto> ClassOfTravels { get; set; }
    }
}
