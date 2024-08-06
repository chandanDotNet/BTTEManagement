using BTTEM.Data.Dto;
using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Command
{
    public class GetVendorCommand : IRequest<ServiceResponse<VendorDto>>
    {
        public Guid Id { get; set; }
    }
}
