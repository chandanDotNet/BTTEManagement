﻿using POS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data
{
    public class ItineraryTicketBookingQuotation : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid ItineraryId { get; set; }
        public string? QuotaionName { get; set; }
        public string? QuotaionPath { get; set; }
        public string? TravelDeskNotes { get; set; }
        public string? RMNotes { get; set; }
    }
}