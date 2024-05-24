using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Dto
{
    public class ContactSupportDto
    {
        public Guid Id { get; set; }
        public string QueryText { get; set; }
        public int ContactNo { get; set; }
        public string DocumentName { get; set; }
        public int IssueNo { get; set; }
        public string IssueText { get; set; }
        public string CustomText { get; set; }
        public string RequestNo { get; set; }
        public bool IsResolved { get; set; }
        public string Notes { get; set; }
        public Guid AssignedTo { get; set; }
    }
}
