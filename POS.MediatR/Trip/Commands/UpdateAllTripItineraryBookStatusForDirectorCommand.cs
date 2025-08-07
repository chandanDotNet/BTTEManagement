using BTTEM.MediatR.Expense.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Trip.Commands
{
    public class UpdateAllTripItineraryBookStatusForDirectorCommand
    {
        //public Guid TripId { get; set; }
        public List<UpdateAllTripItineraryBookStatusCommand> AllTripItineraryBookStatusList { get; set; }
    }
}
