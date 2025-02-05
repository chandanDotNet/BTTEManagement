using BTTEM.Data;
using BTTEM.Data.Resources;
using BTTEM.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.ApprovalLevel.Command
{
    public class GetAllApprovalLevelQuery : IRequest<ApprovalLevelList>
    {
        public ApprovalLevelResource ApprovalLevelResource { get; set; }
    }
}

