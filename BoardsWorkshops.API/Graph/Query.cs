using System;
using System.Collections.Generic;
using System.Linq;
using BoardsWorkshops.API.DataAccess;
using BoardsWorkshops.API.Graph.Cards;
using BoardsWorkshops.API.Graph.Lists;
using BoardsWorkshops.API.Graph.Users;
using HotChocolate;

namespace BoardsWorkshops.API.Graph
{
	public class Query
	{
		public ICollection<TaskList> GetLists([Service] BoardsContext context)
		{
			return context.Lists.ToList();
		}

		public ICollection<Card> GetCards([Service] BoardsContext context)
		{
			return context.Cards.ToList();
		}

		public TaskList GetList(Guid id, [Service] BoardsContext context)
		{
			return context.Lists.First(x => x.Id == id);
		}

		public Card GetCard(Guid id, [Service] BoardsContext context)
		{
			return context.Cards.First(x => x.Id == id);
		}

		public ICollection<User> GetUsers([Service] BoardsContext context)
		{
			return context.Users.ToList();
		}

		public ICollection<User> GetUser(Guid id, [Service] BoardsContext context)
		{
			return context.Users.ToList();
		}
	}
}