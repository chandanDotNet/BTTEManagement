using BTTEM.Data.Dto;
using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Commands
{
    public class UpdateCarBikeLogBookExpenseCommand : IRequest<ServiceResponse<bool>>
    {
        public Guid Id { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string PlaceOfVisitDepartment { get; set; }
        public decimal StartingKMS { get; set; } = 0;
        public decimal EndingKMS { get; set; } = 0;
        public decimal ConsumptionKMS { get; set; } = 0;
        public decimal RefillingLiters { get; set; } = 0;
        public decimal RefillingAmount { get; set; } = 0;
        public string FuelBillNo { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public List<CarBikeLogBookExpenseDocumentDto> Documents { get; set; }
    }
}
