﻿using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CommandAndQuery
{
    public class UpdateExpenseStatusCommand : IRequest<ServiceResponse<bool>>
    {

        public Guid Id { get; set; }
        public string Status { get; set; }
        public string RejectReason { get; set; }
        public decimal PayableAmount { get; set; }      

    }
}
