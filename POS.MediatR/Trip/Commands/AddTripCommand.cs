﻿using BTTEM.Data;
using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CommandAndQuery
{
    public class AddTripCommand : IRequest<ServiceResponse<TripDto>>
    {
       // public string TripNumber { get; set; }
        public string TripNo { get; set; }
        public string TripType { get; set; }
        public string Name { get; set; }
        public DateTime TripStarts { get; set; }
        public DateTime TripEnds { get; set; }
        public Guid PurposeId { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Approval { get; set; }
        public Guid SourceCityId { get; set; }
        public Guid DestinationCityId { get; set; }
        public string MultiCity { get; set; }
        public string ModeOfTrip { get; set; }
        public Guid DepartmentId { get; set; }
        public bool IsRequestAdvanceMoney { get; set; }
        public decimal? AdvanceMoney { get; set; }
        public string? RequestAdvanceMoneyStatus { get; set; }
        public DateTime? RequestAdvanceMoneyDate { get; set; }
    }
}
