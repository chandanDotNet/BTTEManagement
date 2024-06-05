using AutoMapper;
using BTTEM.Data;
using Microsoft.EntityFrameworkCore;
using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Data.Resources;
using POS.Data;
using POS.Domain;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BTTEM.Data.Resources;
using BTTEM.Data.Dto;

namespace BTTEM.Repository
{
    public class StateRepository : GenericRepository<State, POSDbContext>, IStateRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;
        public StateRepository(IUnitOfWork<POSDbContext> uow,
            IPropertyMappingService propertyMappingService,
            IMapper mapper) : base(uow)
        {
            _propertyMappingService = propertyMappingService;
        }
        public async Task<StateList> GetStates(StateResource stateResource)
        {
            var collectionBeforePaging =
                AllIncluding().ApplySort(stateResource.OrderBy,
                _propertyMappingService.GetPropertyMapping<StateDto, State>());

            if (!string.IsNullOrEmpty(stateResource.Name))
            {
                // trim & ignore casing
                var genreForWhereClause = stateResource.Name
                    .Trim().ToLowerInvariant();
                var name = Uri.UnescapeDataString(genreForWhereClause);
                var encodingName = WebUtility.UrlDecode(name);
                var ecapestring = Regex.Unescape(encodingName);
                encodingName = encodingName.Replace(@"\", @"\\").Replace("%", @"\%").Replace("_", @"\_").Replace("[", @"\[").Replace(" ", "%");
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.Name, $"{encodingName}%"));
            }

            var StateList = new StateList();
            return await StateList.Create(collectionBeforePaging, stateResource.Skip, stateResource.PageSize);
        }
    }
}
