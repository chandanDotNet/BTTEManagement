using POS.Data.Resources;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data
{
    public class ApprovalLevelResource : ResourceParameters
    {
        public ApprovalLevelResource() : base("OrderNo")
        {
        }
        public Guid ApprovalLevelTypeId { get; set; }
        public string LevelName { get; set; }
        public Guid RoleId { get; set; }
        public int OrderNo { get; set; }
    }
}
