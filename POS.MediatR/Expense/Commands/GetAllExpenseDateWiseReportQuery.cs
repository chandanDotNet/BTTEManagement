using BTTEM.Data.Entities;
using MediatR;
using POS.Data.Dto;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Commands
{
    public class GetAllExpenseDateWiseReportQuery : IRequest<TourTravelExpenseReport>
    {

        public Guid MasterExpenseId { get; set; }
    }
}
