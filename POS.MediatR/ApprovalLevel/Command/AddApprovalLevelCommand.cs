using BTTEM.Data.Dto;
using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.ApprovalLevel.Command
{
    public class AddApprovalLevelCommand : IRequest<ServiceResponse<ApprovalLevelDto>>
    {
        public Guid? Id { get; set; }
        public Guid ApprovalLevelTypeId { get; set; }
        public string LevelName { get; set; }
        public Guid RoleId { get; set; }        
        public int OrderNo { get; set; }
        //public List<ApprovalLevelUserDto> ApprovalLevelUsers { get; set; }
    }
}
