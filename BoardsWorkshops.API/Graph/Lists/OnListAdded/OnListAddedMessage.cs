using HotChocolate.Subscriptions;

namespace BoardsWorkshops.API.Graph.Lists.OnListAdded
{
    public class OnListAddedMessage : EventMessage
    {
        public OnListAddedMessage(OnListAddedPayload payload) : base(CreateEventDescription(), payload)
        {
        }

        private static EventDescription CreateEventDescription() => new EventDescription("onListAdded");
    }
}