using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Command
{
    public class DeleteVendorCommand : IRequest<ServiceResponse<bool>>
    {
        public Guid Id { get; set; }
    }
}
