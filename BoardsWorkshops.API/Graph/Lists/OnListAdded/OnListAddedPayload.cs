using System;

namespace BoardsWorkshops.API.Graph.Lists.OnListAdded
{
    public class OnListAddedPayload
    {
        public Guid ListId { get; set; }
        public string Name { get; set; }
    }
}