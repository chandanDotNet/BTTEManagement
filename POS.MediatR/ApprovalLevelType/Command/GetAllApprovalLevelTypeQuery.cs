﻿using BTTEM.Data.Resources;
using BTTEM.Repository;
using MediatR;
using POS.Data.Resources;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CommandAndQuery
{
    public class GetAllApprovalLevelTypeQuery : IRequest<ApprovalLevelTypeList>
    {
        public ApprovalLevelTypeResource ApprovalLevelTypeResource { get; set; }
    }
}

