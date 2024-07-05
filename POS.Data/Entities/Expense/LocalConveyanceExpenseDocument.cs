﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Entities
{
    public class LocalConveyanceExpenseDocument
    {

        public Guid Id { get; set; }
        public Guid LocalConveyanceExpenseId { get; set; }
        public string ReceiptName { get; set; }
        public string ReceiptPath { get; set; }
    }
}
