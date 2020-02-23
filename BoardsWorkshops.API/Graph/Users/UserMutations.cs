using BoardsWorkshops.API.DataAccess;
using HotChocolate;

namespace BoardsWorkshops.API.Graph.Users
{
	public class UserMutations
	{
		public User AddUser(AddUserInput input, [Service] BoardsContext context)
		{
			var user = new User
			           {
					           Username = input.Username,
					           Password = input.Password
			           };

			context.Users.Add(user);
			context.SaveChanges();
			
			return user;
		}
	}

	public class AddUserInput
	{
		public string Username { get; set; }
		public string Password { get; set; }
	}
}