
using BTTEM.Data;
using BTTEM.Data.Resources;
using POS.Common.GenericRepository;
using POS.Data.Resources;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Repository
{
    public interface IPoliciesDetailRepository : IGenericRepository<PoliciesDetail>
    {

        Task<PoliciesDetailList> GetPoliciesDetails(PoliciesDetailResource policiesDetailResource);
    }
}
