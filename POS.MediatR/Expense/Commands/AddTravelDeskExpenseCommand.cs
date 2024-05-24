using BTTEM.Data.Dto;
using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CommandAndQuery
{
    public class AddTravelDeskExpenseCommand : IRequest<ServiceResponse<TravelDeskExpenseDto>>
    {
        public Guid? Id { get; set; }       
        public Guid? TripId { get; set; }
        public Guid? TripItineraryId { get; set; }
        public string Name { get; set; }
        public string BillType { get; set; }
        public string Reference { get; set; }       
        public decimal Amount { get; set; }
        public Guid? ExpenseById { get; set; }
        public string Description { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string ReceiptName { get; set; }
        public string DocumentData { get; set; }
        public string Status { get; set; }

    }
}
