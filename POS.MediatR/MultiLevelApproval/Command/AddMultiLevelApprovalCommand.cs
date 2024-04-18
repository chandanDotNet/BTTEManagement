using BTTEM.Data.Dto;
using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CommandAndQuery
{
    public class AddMultiLevelApprovalCommand : IRequest<ServiceResponse<MultiLevelApprovalDto>>
    {
        public string Title { get; set; }
        public bool IsActive { get; set; }
    }
}
