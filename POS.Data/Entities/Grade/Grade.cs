﻿using POS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data
{
    public class Grade : BaseEntity
    {
        public Guid Id { get; set; }
        public string GradeName { get; set; }
        public string Description { get; set; }

        public bool IsActive { get; set; }
    }
}
