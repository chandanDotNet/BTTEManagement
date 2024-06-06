using POS.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data
{
    public class CompanyAccount : BaseEntity
    {
        public Guid Id { get; set; }
        public string AccountName { get; set; }
        public Guid CompanyProfileId { get; set; }
        [ForeignKey("CompanyProfileId")]
        public CompanyProfile CompanyProfile { get; set; }
        public List<CompanyGST> CompanyGST { get; set; }
    }
}
