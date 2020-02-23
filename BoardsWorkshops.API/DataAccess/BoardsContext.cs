using BoardsWorkshops.API.Graph.Cards;
using BoardsWorkshops.API.Graph.Lists;
using BoardsWorkshops.API.Graph.Users;
using Microsoft.EntityFrameworkCore;

namespace BoardsWorkshops.API.DataAccess
{
	public class BoardsContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<TaskList> Lists { get; set; }
		public DbSet<Card> Cards { get; set; }

		public BoardsContext(DbContextOptions<BoardsContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
		}
	}
}