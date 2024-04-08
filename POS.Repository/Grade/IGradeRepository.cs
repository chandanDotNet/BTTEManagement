using POS.Common.GenericRepository;
using BTTEM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTTEM.Data.Resources;

namespace BTTEM.Repository
{
    public interface IGradeRepository : IGenericRepository<Grade>
    {

        Task<GradeList> GetGrades(GradeResource gradeResource);
    }
}
