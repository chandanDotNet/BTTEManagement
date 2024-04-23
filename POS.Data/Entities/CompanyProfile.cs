using BTTEM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data
{
    public class CompanyProfile : BaseEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public string LogoUrl { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string CurrencyCode { get; set; }

        public string? CountryName { get; set; }
        public string? CityName { get; set; }
        public Guid? CountryId { get; set; }
        public Guid? CityId { get; set; }
        public string? ZipCode { get; set; }
        public string? Fax { get; set; }
        public string? WebSite { get; set; }
        public string? FY { get; set; }
        public string? TimeZone { get; set; }
        public string GST { get; set; }
        public bool Registration { get; set; }
        public List<CompanyAccount> CompanyAccounts { get; set; }
    }
}
