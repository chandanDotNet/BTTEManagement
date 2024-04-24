using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.PoliciesTravel.Commands
{
    public class UpdatePoliciesSettingCommand : IRequest<ServiceResponse<bool>>
    {

        public Guid Id { get; set; }
        public Guid PoliciesDetailId { get; set; }
        public int SubmissionDays { get; set; }
        public bool IsDeviationApprovalRequired { get; set; }
        public bool IsActuals { get; set; }
        public bool SetPercentage { get; set; }
        public decimal PercentageAmount { get; set; }
        public bool IsDeleted { get; set; }
    }
}
