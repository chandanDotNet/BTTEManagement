using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Dto
{
    public class GroupUserLimitList
    {
        public decimal MetroCity { get; set; }
        public bool IsMetroCity { get; set; }
        public decimal OtherCity { get; set; }
        public bool IsOtherCity { get; set; }
        public decimal Fooding { get; set; }
        public bool IsFooding { get; set; }
        public decimal DA { get; set; }
    }
}
