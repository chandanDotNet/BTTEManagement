using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.Data.Resources;
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
    public class HelpSupportRepository : GenericRepository<HelpSupport, POSDbContext>, IHelpSupportRepository
    {
        private readonly IMapper _mapper;
        private readonly IPropertyMappingService _propertyMappingService;
        public HelpSupportRepository(IUnitOfWork<POSDbContext> uow, IMapper mapper
            , IPropertyMappingService propertyMappingService) : base(uow)
        {
            _mapper = mapper;
            _propertyMappingService = propertyMappingService;
        }
        public async Task<HelpSupportList> GetAllHelpSupport(HelpSupportResource helpSupportResource)
        {
            var collectionBeforePaging = AllIncluding().ApplySort(helpSupportResource.OrderBy
            ,_propertyMappingService.GetPropertyMapping<HelpSupportDto, HelpSupport>());

            var HelpSupportList = new HelpSupportList();
            return HelpSupportList = await HelpSupportList.Create(collectionBeforePaging, helpSupportResource.Skip, helpSupportResource.PageSize);
        }
    }
}
