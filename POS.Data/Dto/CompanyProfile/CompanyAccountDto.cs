using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Dto
{
    public class CompanyAccountDto
    {
        public Guid Id { get; set; }
        public string AccountName { get; set; }
        public Guid CompanyProfileId { get; set; }
        //public List<CompanyGST> CompanyGST { get; set; }
        public int GSTCount { get; set; }
    }
}
