using System.Security.Cryptography;
using BoardsWorkshops.API.Graph.Cards.OnCardAdded;
using BoardsWorkshops.API.Graph.Lists.OnListAdded;
using HotChocolate.Subscriptions;

namespace BoardsWorkshops.API.Graph
{
    public class Subscription
    {
        public OnCardAddedPayload OnCardAdded(string listId, IEventMessage message)
        {
            return (OnCardAddedPayload) message.Payload;
        }

        public OnListAddedPayload OnListAdded(OnListAddedMessage message)
        {
            return (OnListAddedPayload) message.Payload;
        }
    }
}