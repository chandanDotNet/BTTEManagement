using BTTEM.Data.Resources;
using BTTEM.Repository;
using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.HelpSupport.Command
{
    public class GetAllHelpSupportQuery : IRequest<HelpSupportList>
    {
        public HelpSupportResource HelpSupportResource { get; set; }
    }
}
