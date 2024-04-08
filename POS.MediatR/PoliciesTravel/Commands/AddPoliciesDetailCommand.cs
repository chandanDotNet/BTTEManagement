using BTTEM.Data;
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
    public class AddPoliciesDetailCommand : IRequest<ServiceResponse<PoliciesDetailDto>>
    {

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid GradeId { get; set; }
        public string Description { get; set; }
        public decimal DailyAllowance { get; set; }
        public string Document { get; set; }
    }
}
