﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Trip.Commands
{
    public class ApprovalTripRequestAdvanceMoneyCommand
    {
        public List<UpdateApprovalTripRequestAdvanceMoneyCommand> UpdateApprovalTripRequestAdvanceMoneyCommand { get; set; }
    }
}
