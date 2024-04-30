using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Resources;
using Microsoft.EntityFrameworkCore;
using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BTTEM.Repository
{
    public class EmpGradeRepository : GenericRepository<EmpGrade, POSDbContext>, IEmpGradeRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IUserRepository _userRepository;
        public EmpGradeRepository(
            IUnitOfWork<POSDbContext> uow,
            IPropertyMappingService propertyMappingService,
             IMapper mapper,
             IUserRepository userRepository)
            : base(uow)
        {
            _propertyMappingService = propertyMappingService;
            _userRepository = userRepository;
        }


        public async Task<EmpGradeList> GetEmpGrades(EmpGradeResource empGradeResource)
        {
            var collectionBeforePaging = All;
                //All.ApplySort(empGradeResource.OrderBy, _propertyMappingService.GetPropertyMapping<EmpGradeDto, EmpGrade>());

            if (!string.IsNullOrEmpty(empGradeResource.GradeName))
            {
                // trim & ignore casing
                var genreForWhereClause = empGradeResource.GradeName
                    .Trim().ToLowerInvariant();
                var name = Uri.UnescapeDataString(genreForWhereClause);
                var encodingName = WebUtility.UrlDecode(name);
                var ecapestring = Regex.Unescape(encodingName);
                encodingName = encodingName.Replace(@"\", @"\\").Replace("%", @"\%").Replace("_", @"\_").Replace("[", @"\[").Replace(" ", "%");
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.GradeName, $"{encodingName}%"));
            }

            var CityList = new EmpGradeList(_userRepository);
            return await CityList.Create(collectionBeforePaging, empGradeResource.Skip, empGradeResource.PageSize);
        }

    }
}
