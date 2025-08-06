using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data
{
    public class Sap : BaseEntity
    {
        public Guid Id { get; set; }
        public string SapData { get; set; }
        public string Status { get; set; }
        public int DocumentNumber { get; set; }
    }
}
