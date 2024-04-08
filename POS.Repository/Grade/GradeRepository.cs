using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.Data.Resources;
using Microsoft.EntityFrameworkCore;
using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Data.Resources;
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
    public class GradeRepository : GenericRepository<Grade, POSDbContext>, IGradeRepository
    {

        private readonly IPropertyMappingService _propertyMappingService;

        public GradeRepository(
            IUnitOfWork<POSDbContext> uow,
            IPropertyMappingService propertyMappingService,
             IMapper mapper)
            : base(uow)
        {
            _propertyMappingService = propertyMappingService;
        }


        public async Task<GradeList> GetGrades(GradeResource gradeResource)
        {
            var collectionBeforePaging = All;
                //All.ApplySort(gradeResource.OrderBy,_propertyMappingService.GetPropertyMapping<GradeDto, Grade>());

            if (!string.IsNullOrEmpty(gradeResource.GradeName))
            {
                // trim & ignore casing
                var genreForWhereClause = gradeResource.GradeName
                    .Trim().ToLowerInvariant();
                var name = Uri.UnescapeDataString(genreForWhereClause);
                var encodingName = WebUtility.UrlDecode(name);
                var ecapestring = Regex.Unescape(encodingName);
                encodingName = encodingName.Replace(@"\", @"\\").Replace("%", @"\%").Replace("_", @"\_").Replace("[", @"\[").Replace(" ", "%");
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.GradeName, $"{encodingName}%"));
            }            

            var CityList = new GradeList();
            return await CityList.Create(collectionBeforePaging, gradeResource.Skip, gradeResource.PageSize);
        }
    }
}
