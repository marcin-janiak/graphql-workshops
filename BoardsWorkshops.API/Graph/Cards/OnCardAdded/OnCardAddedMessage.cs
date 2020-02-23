using HotChocolate.Language;
using HotChocolate.Subscriptions;

namespace BoardsWorkshops.API.Graph.Cards.OnCardAdded
{
    public class OnCardAddedMessage : EventMessage
    {
        public OnCardAddedMessage(OnCardAddedPayload payload) : base(CreateEventDescription(payload.ListId.ToString()),
            payload)
        {
        }

        private static EventDescription CreateEventDescription(string listId) =>
            new EventDescription("onCardAdded", new ArgumentNode("listId", new StringValueNode(listId)));
    }
}