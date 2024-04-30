using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.City.Commands
{
    public class UpdateMultipleCityCommand : IRequest<ServiceResponse<bool>>
    {
        public List<CityDto> Cities { get; set; }
    }
}
