using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CommandAndQuery
{
    public class MMTTripQueryCommand
    {
        [JsonProperty("expense-client-id")]
        public string expenseclientid { get; set; }

        [JsonProperty("external-org-id")]
        public string externalorgid { get; set; }

        [JsonProperty("to-date")]
        public long todate { get; set; }

        [JsonProperty("from-date")]
        public long fromdate { get; set; }

        [JsonProperty("report-type")]
        public string reporttype { get; set; }
        public string level { get; set; }
    }
}
