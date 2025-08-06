using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.API.Helpers
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'QueuePriority'
    public static class QueuePriority
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'QueuePriority'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'QueuePriority.Critical'
        public const string Critical = "critical";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'QueuePriority.Critical'
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'QueuePriority.Default'
        public const string Default = "default";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'QueuePriority.Default'
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'QueuePriority.Passive'
        public const string Passive = "passive";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'QueuePriority.Passive'

        //The order is important, workers will fetch jobs from the
        //critical queue first, and then from the default queue, and so on...
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'QueuePriority.Priority'
        public static string[] Priority = new[] { Critical, Default, Passive };
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'QueuePriority.Priority'
    }
}
