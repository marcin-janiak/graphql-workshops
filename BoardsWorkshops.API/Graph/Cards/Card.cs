using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoardsWorkshops.API.DataAccess;
using BoardsWorkshops.API.Graph.Lists;
using BoardsWorkshops.API.Graph.Users;
using GreenDonut;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace BoardsWorkshops.API.Graph.Cards
{
    public interface ICard
    {
        public Guid Id { get; set; }
        public Guid ListId { get; set; }
        public string? Description { get; set; }
        public Guid CreatedBy { get; set; }
    }

    public interface IAssignedUsersCard
    {
    }

    public interface ITodoCard
    {
    }

    public interface IDeadlineCard
    {
    }

    public class Card
    {
        public Guid Id { get; set; }
        public Guid ListId { get; set; }
        public string? Description { get; set; }
        public Guid CreatedBy { get; set; }
        public string Name { get; set; }

        public Task<TaskList> TaskList([Service] BoardsContext context, [Parent] Card card,
            [Service] IResolverContext resolverContext)
        {
            IDataLoader<Guid, TaskList> dataLoader = resolverContext.BatchDataLoader<Guid, TaskList>("GetListById",
                async keys =>
                    await context.Lists.Where(u => keys.Contains(u.Id))
                        .ToDictionaryAsync(list => list.Id, list => list)
            );

            return dataLoader.LoadAsync(card.ListId);
        }
    }

    public class CardType : ObjectType<Card>
    {
        protected override void Configure(IObjectTypeDescriptor<Card> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Field("createdBy")
                .Resolver(
                    x =>
                    {
                        var parent = x.Parent<Card>();

                        var context = x.Service<BoardsContext>();
                        IDataLoader<Guid, User> dataLoader = x.BatchDataLoader<Guid, User>("GetUserById",
                            async keys =>
                                await context.Users.Where(u => keys.Contains(u.Id))
                                    .ToDictionaryAsync(user => user.Id, user => user)
                        );

                        return dataLoader.LoadAsync(parent.CreatedBy);

                        return context.Users.First(x => x.Id == parent.CreatedBy);
                    }
                ).Type(typeof(Task<User>));
        }
    }
}