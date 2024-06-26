﻿using POS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Dto
{
    public class NotificationDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid SourceId { get; set; }
        public string Content { get; set; }
        public int Read { get; set; }
        public DateTime CreatedDate { get; set; }
        public User SourceUser { get; set; }
        public Guid TripId { get; set; }
        public string TypeName { get; set; }
    }
}
