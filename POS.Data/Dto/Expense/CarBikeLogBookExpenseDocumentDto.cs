using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Dto
{
    public class CarBikeLogBookExpenseDocumentDto
    {

        public Guid Id { get; set; }
        public Guid CarBikeLogBookExpenseId { get; set; }
        public string ReceiptName { get; set; }
        public string ReceiptPath { get; set; }
    }
}
