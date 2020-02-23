using BoardsWorkshops.API.Graph.Cards;
using BoardsWorkshops.API.Graph.Lists;
using BoardsWorkshops.API.Graph.Users;
using HotChocolate.Types;

namespace BoardsWorkshops.API.Graph
{
	public class Mutation
	{
		public MeMutations Me => new MeMutations();
	}

	public class MutationType : ObjectType<Mutation>
	{
		protected override void Configure(IObjectTypeDescriptor<Mutation> descriptor)
		{
			base.Configure(descriptor);
			descriptor.Include<UserMutations>();
		}
	}

	public class MeMutations
	{
	}

	public class MeMutationsType : ObjectType<MeMutations>
	{
		protected override void Configure(IObjectTypeDescriptor<MeMutations> descriptor)
		{
			base.Configure(descriptor);

			descriptor.Authorize();

			descriptor.Include<ListMutations>();
			descriptor.Include<CardMutations>();
		}
	}
}