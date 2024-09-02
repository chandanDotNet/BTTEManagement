using BTTEM.Data;
using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Repository
{
    public class CancelTripItineraryHotelUserRepository : GenericRepository<CancelTripItineraryHotelUser, POSDbContext>, ICancelTripItineraryHotelUserRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;

        public CancelTripItineraryHotelUserRepository(IUnitOfWork<POSDbContext> uow,
            IPropertyMappingService propertyMappingService) : base(uow)
        {
            _propertyMappingService = propertyMappingService;
        }
    }
}