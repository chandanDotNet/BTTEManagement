using BTTEM.Data.Entities;
using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.CommandAndQuery
{
    public class AddSapCommand : IRequest<ServiceResponse<SapDto>>
    {
        public Guid Id { get; set; }
        public Guid MasterExpenseId { get; set; }
        public string SapData { get; set; }
        public string Status { get; set; }
        public string DocumentNumber { get; set; }
        public int JourneyNumber { get; set; }
        public string Message { get; set; }
        public string MessageReturn { get; set; }
    }

    public class SapCommand 
    {
        public SapRecord Record { get; set; }
    }
    public class SapRecord
    {
        public string AuthorizationKey { get; set; }
        public string CompanyCode { get; set; }
        public string UserName { get; set; }
        public Guid MasterExpenseId { get; set; }
        public WithTax WithTax { get; set; }
        public WithoutTax WithoutTax { get; set; }

        //public WithoutTax WithoutTax { get; set; }
    }
    public class WithoutTax
    {
        public string Supplier { get; set; }
        public string HODate { get; set; }
        public string DocumentDate { get; set; }
        public string PostingDate { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Persons { get; set; }
        public string TravelType { get; set; }
        public string CostCenter { get; set; }
        public string BusinessPlace { get; set; }
        public string Assignment { get; set; }
        public string Reference { get; set; }
        public string TradingPartner { get; set; }
        public string Remarks { get; set; }
        public string BusinessArea { get; set; }
        public string Fare { get; set; }
        public string Fooding { get; set; }
        public string Lodging { get; set; }
        public string Conveyance { get; set; }
        public string OtherExpense { get; set; }
        public string Network { get; set; }
        public string ActivityNo { get; set; }
    }

    public class WithTax
    {
        public string JourneyNo { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string DocumentDate { get; set; }
        public string PostingDate { get; set; }
        public string HODate { get; set; }
        public string Persons { get; set; }
        public string TravelType { get; set; }
        public string Reference { get; set; }
        public string TradingPartner { get; set; }
        public string Assignment { get; set; }
        public string Remarks { get; set; }
        public string SACCode { get; set; }
        public string Network { get; set; }
        public string ActivityNo { get; set; }
        public string BikeMaintenance { get; set; }
        public string CreditNote { get; set; }
        public List<Item> Item { get; set; } = new List<Item>();
    }

    //public class Item1
    //{
    //    public string Supplier { get; set; }
    //    public string Amount { get; set; }
    //    public string BaseAmount { get; set; }
    //    public string TaxCode { get; set; }
    //    public string SubVendor { get; set; }
    //    public string TaxGSTSubVendor { get; set; }
    //    public string MainVendorIRNApplicable { get; set; }
    //    public string SubVendorIRNApplicable { get; set; }
    //    public string SACCode1 { get; set; }
    //    public string SACCode2 { get; set; }
    //    public string RefDocNo { get; set; }
    //    public string DocPostingDate { get; set; }
    //}
    //public class Item2
    //{
    //    public string Supplier { get; set; }
    //    public string Amount { get; set; }
    //    public string BaseAmount { get; set; }
    //    public string TaxCode { get; set; }
    //    public string SubVendor { get; set; }
    //    public string TaxGSTSubVendor { get; set; }
    //    public string MainVendorIRNApplicable { get; set; }
    //    public string SubVendorIRNApplicable { get; set; }
    //    public string SACCode1 { get; set; }
    //    public string SACCode2 { get; set; }
    //    public string RefDocNo { get; set; }
    //    public string DocPostingDate { get; set; }
    //}
    //public class Item3
    //{
    //    public string Supplier { get; set; }
    //    public string Amount { get; set; }
    //    public string BaseAmount { get; set; }
    //    public string TaxCode { get; set; }
    //    public string SubVendor { get; set; }
    //    public string TaxGSTSubVendor { get; set; }
    //    public string MainVendorIRNApplicable { get; set; }
    //    public string SubVendorIRNApplicable { get; set; }
    //    public string SACCode1 { get; set; }
    //    public string SACCode2 { get; set; }
    //    public string RefDocNo { get; set; }
    //    public string DocPostingDate { get; set; }
    //}
    //public class Item4
    //{
    //    public string Supplier { get; set; }
    //    public string Amount { get; set; }
    //    public string BaseAmount { get; set; }
    //    public string TaxCode { get; set; }
    //    public string SubVendor { get; set; }
    //    public string TaxGSTSubVendor { get; set; }
    //    public string MainVendorIRNApplicable { get; set; }
    //    public string SubVendorIRNApplicable { get; set; }
    //    public string SACCode1 { get; set; }
    //    public string SACCode2 { get; set; }
    //    public string RefDocNo { get; set; }
    //    public string DocPostingDate { get; set; }
    //}
    //public class Item5
    //{
    //    public string Supplier { get; set; }
    //    public string Amount { get; set; }
    //    public string BaseAmount { get; set; }
    //    public string TaxCode { get; set; }
    //    public string SubVendor { get; set; }
    //    public string TaxGSTSubVendor { get; set; }
    //    public string MainVendorIRNApplicable { get; set; }
    //    public string SubVendorIRNApplicable { get; set; }
    //    public string SACCode1 { get; set; }
    //    public string SACCode2 { get; set; }
    //    public string RefDocNo { get; set; }
    //    public string DocPostingDate { get; set; }
    //}
    //public class Item6
    //{
    //    public string Supplier { get; set; }
    //    public string Amount { get; set; }
    //    public string BaseAmount { get; set; }
    //    public string TaxCode { get; set; }
    //    public string SubVendor { get; set; }
    //    public string TaxGSTSubVendor { get; set; }
    //    public string MainVendorIRNApplicable { get; set; }
    //    public string SubVendorIRNApplicable { get; set; }
    //    public string SACCode1 { get; set; }
    //    public string SACCode2 { get; set; }
    //    public string RefDocNo { get; set; }
    //    public string DocPostingDate { get; set; }
    //}
    //public class WithoutTax
    //{
    //    public string Supplier { get; set; }
    //    public string HODate { get; set; }
    //    public string DocumentDate { get; set; }
    //    public string PostingDate { get; set; }
    //    public string StartDate { get; set; }
    //    public string EndDate { get; set; }
    //    public string Persons { get; set; }
    //    public string TravelType { get; set; }
    //    public string CostCenter { get; set; }
    //    public string BusinessPlace { get; set; }
    //    public string Assignment { get; set; }
    //    public string Reference { get; set; }
    //    public string TradingPartner { get; set; }
    //    public string Remarks { get; set; }
    //    public string BusinessArea { get; set; }
    //    public string Fare { get; set; }
    //    public string Fooding { get; set; }
    //    public string Lodging { get; set; }
    //    public string Conveyance { get; set; }
    //    public string OtherExpense { get; set; }
    //    public string Network { get; set; }
    //    public string ActivityNo { get; set; }
    //}

    //public class WithTax
    //{
    //    public string JourneyNo { get; set; }
    //    public string StartDate { get; set; }
    //    public string EndDate { get; set; }
    //    public string DocumentDate { get; set; }
    //    public string PostingDate { get; set; }
    //    public string HODate { get; set; }
    //    public string Persons { get; set; }
    //    public string TravelType { get; set; }
    //    public string Reference { get; set; }
    //    public string CostCenter { get; set; }
    //    public string TradingPartner { get; set; }
    //    public string Assignment { get; set; }
    //    public string Remarks { get; set; }
    //    public string SACCode { get; set; }
    //    public string Network { get; set; }
    //    public string ActivityNo { get; set; }
    //    public string Fare { get; set; }
    //    public string Fooding { get; set; }
    //    public string Lodging { get; set; }
    //    public string Conveyance { get; set; }
    //    public string OtherExpense { get; set; }
    //    public string BusinessPlace { get; set; }
    //    public string BusinessArea { get; set; }
    //    public Item1 Item1 { get; set; }
    //    public Item2 Item2 { get; set; }
    //    public Item3 Item3 { get; set; }
    //    public Item4 Item4 { get; set; }
    //    public Item5 Item5 { get; set; }
    //    public Item6 Item6 { get; set; }
    //}
}
