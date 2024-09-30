using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Dto
{
    public class AppVersionUpdateDto
    {
        public Guid Id { get; set; }
        public string AppType { get; set; }
        public string Version { get; set; }
    }
}
