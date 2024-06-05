using BTTEM.Data.Resources;
using BTTEM.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Branch.Command
{
    public class GetAllBranchQueryCommand : IRequest<BranchList>
    {
        public BranchResource BranchResource { get; set; }
    }
}
