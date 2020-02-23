using System;

namespace BoardsWorkshops.API.Graph.Cards.OnCardAdded
{
    public class OnCardAddedPayload
    {
        public Guid Id { get; set; }
        public Guid ListId { get; set; }
        public string? Description { get; set; }
        public string Name { get; set; }
    }
}