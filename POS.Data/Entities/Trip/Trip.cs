﻿using POS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data
{
    public class Trip : BaseEntity
    {

        public Guid Id { get; set; } 
        public string TripNo { get; set; }    
        public string TripType { get; set; } 
        public string Name { get; set; }
        public DateTime TripStarts { get; set; }
        public DateTime TripEnds { get; set; }
        public Guid PurposeId { get; set; }
        public string Description { get; set; }
        public Purpose Purpose { get; set; }


    }
}
