﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Dto
{
    public class CancelTripItineraryHotelUserDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid TripItineraryId { get; set; }
        public bool IsCancelrequest { get; set; }
    }
}
