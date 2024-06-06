using MediatR;
using Microsoft.Identity.Client;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Branch.Command
{
    public class DeleteBranchCommand : IRequest<ServiceResponse<bool>>
    {
        public Guid Id { get; set; }
    }
}
