using POS.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Dto
{
    public class TravelDeskExpenseDto
    {

        public Guid Id { get; set; }       
        public Guid? TripId { get; set; }
        public Guid? TripItineraryId { get; set; }
        public string Name { get; set; }
        public string BillType { get; set; }
        public string Reference { get; set; }       
        public decimal Amount { get; set; }
        public Guid? ExpenseById { get; set; }
        public string Description { get; set; }
        public UserDto ExpenseBy { get; set; }
        public DateTime ExpenseDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ReceiptName { get; set; }
        public string ReceiptPath { get; set; }
        public string Status { get; set; }
        public string AccountStatus { get; set; }
        public string AccountStatusRemarks { get; set; }
    }
}
