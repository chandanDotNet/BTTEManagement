using BTTEM.Data.Dto;
using BTTEM.Data.Entities;
using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.ApprovalLevel.Command
{
    public class AddApprovalLevelTypeCommand : IRequest<ServiceResponse<ApprovalLevelTypeDto>>
    {
        public Guid? Id { get; set; }
        public string TypeName { get; set; }
        public Guid CompanyId { get; set; }
    }
}
