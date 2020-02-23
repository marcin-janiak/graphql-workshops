using System;
using System.Linq;
using BoardsWorkshops.API.DataAccess;
using BoardsWorkshops.API.Graph.Lists.OnListAdded;
using HotChocolate;
using HotChocolate.Subscriptions;

namespace BoardsWorkshops.API.Graph.Lists
{
    public class ListMutations
    {
        public TaskList ChangeName(Guid listId, string name, [Service] BoardsContext context)
        {
            var list = context.Lists.First(x => x.Id == listId);

            list.Name = name;

            context.Lists.Update(list);
            context.SaveChanges();

            return list;
        }

        public TaskList AddList(AddListInput input, [Service] BoardsContext context,
            [AuthenticatedUserIdState] Guid userId, [Service] IEventSender eventSender)
        {
            var list = new TaskList
            {
                Name = input.Name,
                CreatedBy = userId
            };

            context.Lists.Add(list);
            context.SaveChanges();

            eventSender.SendAsync(new OnListAddedMessage(new OnListAddedPayload
            {
                Name = list.Name,
                ListId = list.Id
            }));

            return list;
        }
    }

    public class AddListInput
    {
        public string Name { get; set; }
    }
}