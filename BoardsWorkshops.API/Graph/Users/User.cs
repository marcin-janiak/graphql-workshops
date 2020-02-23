using System;
using System.Linq;
using BoardsWorkshops.API.DataAccess;
using BoardsWorkshops.API.Graph.Lists;
using HotChocolate.Types;

namespace BoardsWorkshops.API.Graph.Users
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UserType : ObjectType<User>
    {
        protected override void Configure(IObjectTypeDescriptor<User> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Field("lists")
                .Resolver(
                    x =>
                    {
                        var parent = x.Parent<User>();

                        var context = x.Service<BoardsContext>();

                        return context.Lists.Where(x => x.CreatedBy == parent.Id);
                    }
                )
                .Type<ListType<TaskListType>>();


            descriptor.Field("cards")
                .Resolver(
                    x =>
                    {
                        var parent = x.Parent<User>();

                        var context = x.Service<BoardsContext>();

                        return context.Cards.Where(x => x.CreatedBy == parent.Id);
                    }
                ).Type<ListType<BoardsWorkshops.API.Graph.Cards.CardType>>();
            ;
        }
    }
}