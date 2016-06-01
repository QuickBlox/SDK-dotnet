using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Quickblox.Sdk.Modules.NotificationModule.Models;

namespace Quickblox.Sdk.Modules.NotificationModule.Responses
{
    public class CreateSubscriptionResponseItem
    {
        [JsonProperty("subscription")]
        public Subscription Subscription { get; set; }
    }
}
