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
    public interface ITripTrackingRepository : IGenericRepository<TripTracking>
    {
        Task<TripTrackingList> GetTripTrackings(TripTrackingResource tripTrackingResource);
    }
}
