﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data
{
    public class CancelTripItineraryHotelUser
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid TripItineraryId { get; set; }
        public bool IsCancelrequest { get; set; }
        public bool? IsHotel { get; set; } = false;
    }
}