﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Dto.PoliciesTravel
{
    public class ClassOfTravelDto
    {
        public Guid Id { get; set; }
        public Guid TravelModeId { get; set; }
        public string ClassName { get; set; }
        public bool IsCheck { get; set; }
        public bool IsDeleted { get; set; }

    }
}
