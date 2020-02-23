using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoardsWorkshops.API.DataAccess;
using BoardsWorkshops.API.Graph.Cards;
using BoardsWorkshops.API.Graph.Users;
using GreenDonut;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;

namespace BoardsWorkshops.API.Graph.Lists
{
    public class TaskList
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CreatedBy { get; set; }

        public async Task<ICollection<Card>> Cards([Parent] TaskList list, [Service] BoardsContext context,
            [Service] IResolverContext resolverContext)
        {
            IDataLoader<Guid, Card[]> dataLoader = resolverContext.GroupDataLoader<Guid, Card>("GetCardsByListId",
                 keys =>
                    Task.FromResult(context.Cards.Where(u => keys.Contains(u.ListId)).ToLookup(key => key.ListId, card => card))
            );

            return await dataLoader.LoadAsync(list.Id);
        }
    }

    public class TaskListType : ObjectType<TaskList>
    {
        protected override void Configure(IObjectTypeDescriptor<TaskList> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Field("createdBy")
                .Resolver(
                    x =>
                    {
                        var parent = x.Parent<TaskList>();

                        var context = x.Service<BoardsContext>();

                        IDataLoader<Guid, User> dataLoader = x.BatchDataLoader<Guid, User>("GetUserById",
                            keys =>
                                Task.FromResult(
                                    context.Users.Where(u => keys.Contains(u.Id)).ToDictionary(x => x.Id, user => user)
                                        as IReadOnlyDictionary<Guid, User>)
                        );

                        return dataLoader.LoadAsync(parent.CreatedBy);
                    }
                );
        }
    }
}