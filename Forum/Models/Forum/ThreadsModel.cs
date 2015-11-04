using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum.Models.Forum
{
	public class ThreadsModel
	{
		public ThreadsModel(string[] threadsName)
		{
			ThreadNames = threadsName;
		}

		public string[] ThreadNames { get; private set; }
	}
}