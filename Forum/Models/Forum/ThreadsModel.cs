using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum.Models.Forum
{
	public class ThreadsModel
	{
		public ThreadsModel(Dictionary<string, string[]> threads, bool isAdmin)
		{
			Threads = threads;
			IsAdmin = isAdmin;
		}

		public bool IsAdmin { get; set; }

		public Dictionary<string, string[]> Threads { get; private set; }
	}
}