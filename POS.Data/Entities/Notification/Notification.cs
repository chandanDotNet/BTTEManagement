using POS.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data
{
    public class Notification : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid SourceId { get; set; }
        [ForeignKey("SourceId")]
        public User SourceUser { get; set; }
        public string Content { get; set; }
        public int Read { get; set; }
        

    }
}
