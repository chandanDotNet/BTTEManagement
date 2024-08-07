using BTTEM.Data.Dto;
using BTTEM.Data.Entities;
using POS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data
{
    public class CarBikeLogBookExpense :BaseEntity
    {
        public Guid Id { get; set; }
        public Guid MasterExpenseId { get; set; }
        public string ExpenseType { get; set; }
        public string FuelType { get; set; }
        public DateTime ExpenseDateFrom { get; set; }
        public DateTime ExpenseDateTo { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string PlaceOfVisitDepartment { get; set; }
        public decimal StartingKMS { get; set; } = 0;
        public decimal EndingKMS { get; set; } = 0;
        public decimal ConsumptionKMS { get; set; } = 0;
        public decimal RefillingLiters { get; set; } = 0;
        public decimal RefillingAmount { get; set; } = 0;
        public List<CarBikeLogBookExpenseRefillingDocument> RefillingDocuments { get; set; }
        public string FuelBillNo { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string TollParking { get; set; }
        public List<CarBikeLogBookExpenseTollParkingDocument> TollParkingDocuments { get; set; }
        public List<CarBikeLogBookExpenseDocument> Documents { get; set; }

    }
}
