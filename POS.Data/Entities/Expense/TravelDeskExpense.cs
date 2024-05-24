using Microsoft.EntityFrameworkCore.Metadata.Internal;
using POS.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data
{
    public class TravelDeskExpense : BaseEntity
    {
        public Guid Id { get; set; }      
        public Guid? TripId { get; set; }
        public Guid? TripItineraryId { get; set; }
        public string Name { get; set; }
        public string BillType { get; set; }
        public string Reference { get; set; }
      
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public Guid? ExpenseById { get; set; }
        public string Description { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string ReceiptName { get; set; }
        public string ReceiptPath { get; set; }
        [ForeignKey("ExpenseById")]
        public User ExpenseBy { get; set; }
        public string Status { get; set; }
        public string AccountStatus { get; set; }
        public string AccountStatusRemarks { get; set; }

    }
}
