using POS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data
{
    public class BusinessArea : BaseEntity
    {
        public Guid? Id { get; set; }
        public string CostCenterBranchName { get; set; }
        public string BusinessAreaStateName { get; set; }
        public string BusinessAreaName { get; set; }
        public string BusinessPlace { get; set; }
        public string ProfitCenter { get; set; }
        public Guid? CompanyAccountId { get; set; }
    }
}
