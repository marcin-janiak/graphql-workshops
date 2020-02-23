using System;
using System.Threading.Tasks;
using BoardsWorkshops.API.Graph.Users;
using Microsoft.EntityFrameworkCore;

namespace BoardsWorkshops.API.DataAccess
{
	public interface IUserRepository
	{
		Task AddAsync(User user);
		Task<User> GetByUsernameAsync(string username);
	}

	public class UserRepository : IUserRepository
	{
		private readonly BoardsContext _context;

		public UserRepository(BoardsContext context)
		{
			_context = context;
		}

		public async Task AddAsync(User user)
		{
			await _context.Users.AddAsync(user);

			await _context.SaveChangesAsync();
		}

		public Task<User> GetByUsernameAsync(string username) =>
				_context.Users.FirstAsync(x => x.Username.ToLower() == username.ToLower());

		public Task<User> GetSingleAsync(Guid userId)
		{
			return _context.Users.FirstAsync(x => x.Id == userId);
		}
	}
}