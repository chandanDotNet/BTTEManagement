using BTTEM.Data;
using BTTEM.Data.Resources;
using BTTEM.Repository.Expense;
using POS.Common.GenericRepository;
using POS.Data.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Repository
{
    public interface ITripRepository : IGenericRepository<Trip>
    {

        Task<TripList> GetAllTrips(TripResource tripResource);
    }
}
