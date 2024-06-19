using BTTEM.Data.Entities;
using MediatR;
using POS.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CommandAndQuery
{
    public class GetYearlyExpenseQuery : IRequest<List<YearlyExpenseReportList>>
    {

        public int Year { get; set; }
        public Guid? CompanyAccountId { get; set; }
    }
}
