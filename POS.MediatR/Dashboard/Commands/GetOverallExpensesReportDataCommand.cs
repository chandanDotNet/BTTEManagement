﻿using BTTEM.Data.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Dashboard.Commands
{
    public class GetOverallExpensesReportDataCommand : IRequest<AllTypeReports>
    {
        public Guid? CompanyId { get; set; }
        public int Year { get; set; } = 0;
        public int Month { get; set; } = 0;


    }
}
