using BTTEM.Data.Resources;
using BTTEM.Data;
using POS.Common.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Repository
{
    public interface IApprovalLevelRepository : IGenericRepository<ApprovalLevel>
    {
        Task<ApprovalLevelList> GetApprovalLevels(ApprovalLevelResource approvalLevelResource);
    }
}
