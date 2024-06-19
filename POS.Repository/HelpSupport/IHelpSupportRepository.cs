using BTTEM.Data;
using BTTEM.Data.Resources;
using POS.Common.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Repository
{
    public interface IHelpSupportRepository : IGenericRepository<HelpSupport>
    {
        Task<HelpSupportList> GetAllHelpSupport(HelpSupportResource helpSupportResource);
    }
}
