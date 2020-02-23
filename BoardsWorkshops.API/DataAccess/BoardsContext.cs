using Microsoft.EntityFrameworkCore;

namespace BoardsWorkshops.API.DataAccess
{
	public class BoardsContext : DbContext
	{
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