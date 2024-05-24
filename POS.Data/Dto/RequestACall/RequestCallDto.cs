﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Dto
{
    public class RequestCallDto
    {
        public Guid Id { get; set; }
        public string RequestText { get; set; }
        public int ContactNo { get; set; }
        public string DocumentName { get; set; }
        public DateTime RequestCallDate { get; set; }
        public string RequestCallTime { get; set; }
        public string RequestNo { get; set; }
        public bool IsResolved { get; set; }
        public string Notes { get; set; }
    }
}
