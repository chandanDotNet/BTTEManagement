using BTTEM.Data.Dto;
using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Branch.Command
{
    public class AddBranchCommand : IRequest<ServiceResponse<BranchDto>>
    {
        public string Name { get; set; }
    }
}
