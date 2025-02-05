using POS.Common.GenericRepository;
using BTTEM.Data.Resources;
using BTTEM.Data;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Repository
{
    public interface IApprovalLevelTypeRepository : IGenericRepository<ApprovalLevelType>
    {
        Task<ApprovalLevelTypeList> GetApprovalLevelTypes(ApprovalLevelTypeResource approvalLevelTypeResource);
    }
}
