using BTTEM.Data.Resources;
using BTTEM.Repository;
using POS.Common.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTTEM.Data;

namespace BTTEM.Repository
{
    public interface IAdvanceMoneyRepository : IGenericRepository<Trip>
    {
        Task<AdvanceMoneyList> GetAllAdvanceMoney(AdvanceMoneyResource advanceMoneyResource);
    }
}
