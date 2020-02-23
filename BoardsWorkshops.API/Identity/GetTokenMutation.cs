using System.Threading.Tasks;
using BoardsWorkshops.API.Graph;
using HotChocolate;
using HotChocolate.Types;

namespace BoardsWorkshops.API.Identity
{
	public class GetTokenMutation
	{
		public Task<string> GetToken(string username, string password, [Service] IIdentityService identityService) =>
				identityService.Authenticate(username, password);
	}

	public class GetTokenMutationType : ObjectTypeExtension<Mutation>
	{
		protected override void Configure(IObjectTypeDescriptor<Mutation> descriptor)
		{
			base.Configure(descriptor);

			descriptor.Include<GetTokenMutation>();
		}
	}
}