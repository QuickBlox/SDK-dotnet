using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quickblox.Sdk;
using Quickblox.Sdk.GeneralDataModel.Response;
using Quickblox.Sdk.Modules.NotificationModule.Models;
using Quickblox.Sdk.Modules.NotificationModule.Requests;

namespace TestRequest.ViewModel
{
    public class ChatsViewModel : ViewModel
    {
        private readonly QuickbloxClient client;
        private HttpResponse<EventResponse> createSubscriptionsResponse;
        private CreateEventRequest createEventRequest;

        public ChatsViewModel(QuickbloxClient client)
        {
            this.client = client;
            this.Load();
        }

        private async Task Load()
        {
            createEventRequest = new CreateEventRequest();
            createEventRequest.Event = new CreateEvent()
            {
                NotificationType = NotificationType.push,
                Environment = Environment.production,
                Message = "I love quickblox"
            };
            createSubscriptionsResponse = await this.client.NotificationClient.CreateEvent(createEventRequest);
        }
    }
}
