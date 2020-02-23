using HotChocolate;

namespace BoardsWorkshops.API.Graph
{
	public class AuthenticatedUserIdState : GlobalStateAttribute
	{
		public AuthenticatedUserIdState() : base("UserId")
		{
		}
	}
}