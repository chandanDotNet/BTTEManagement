using BTTEM.Data;
using POS.Common.GenericRepository;
using BTTEM.Data.Resources;
using POS.Repository;
using System.Threading.Tasks;

namespace BTTEM.Repository
{
    public interface IMMTCityRepository : IGenericRepository<MMTCity>
    {
        Task<MMTCityList> GetMMTCities(MMTCityResource mmtCityResource);
    }
}

