﻿using System;

namespace POS.Data
{
    public class ExpenseCategory: BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
