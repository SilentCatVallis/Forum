using System;
using System.Collections.Generic;
using System.Linq;

namespace Forum.Models.Forum
{
	public class UsersModel
	{
		public UsersModel(IEnumerable<String> usersName, HashSet<string> admins, bool isAdmin)
		{
			Names = usersName.ToArray();
			Admins = admins;
			IsAdmin = isAdmin;
		}

		public bool IsAdmin { get; set; }

		public HashSet<string> Admins { get; set; }

		public string[] Names { get; set; }
	}
}