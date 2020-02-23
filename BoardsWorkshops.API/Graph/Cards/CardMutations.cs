using System;
using System.Linq;
using BoardsWorkshops.API.DataAccess;
using BoardsWorkshops.API.Graph.Cards.OnCardAdded;
using HotChocolate;
using HotChocolate.Subscriptions;

namespace BoardsWorkshops.API.Graph.Cards
{
    public class CardMutations
    {
        public Card AddCard(AddCardInput input, [Service] BoardsContext context, [AuthenticatedUserIdState] Guid userId,
            [Service]  IEventSender eventSender)
        {
            var card = new Card()
            {
                Name = input.Name,
                CreatedBy = userId,
                Description = input.Description,
                ListId = input.ListId,
            };

            context.Cards.Add(card);
            context.SaveChanges();

            eventSender.SendAsync(new OnCardAddedMessage(
                new OnCardAddedPayload
                {
                    Description = card.Description,
                    Id = card.Id,
                    Name = card.Name,
                    ListId = card.ListId
                }));

            return card;
        }

        public Card UpdateCard(UpdateCardInput input, [Service] BoardsContext context)
        {
            var card = context.Cards.First(x => x.Id == input.CardId);
            card.Description = input.Description;
            card.Name = input.Name;

            context.Cards.Update(card);
            context.SaveChanges();

            return card;
        }

        public Card MoveCard(Guid cardId, Guid targetListId, [Service] BoardsContext context)
        {
            var card = context.Cards.First(x => x.Id == cardId);

            card.ListId = targetListId;

            context.Cards.Update(card);
            context.SaveChanges();

            return card;
        }
    }

    public class AddCardInput
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public Guid ListId { get; set; }
    }

    public class UpdateCardInput
    {
        public string Name { get; set; }
        public Guid CardId { get; set; }
        public string? Description { get; set; }
    }
}