using System;
using System.Linq;
using System.Web.Mvc;
using Forum.Forum;
using Forum.Models;
using Forum.Models.Forum;

namespace Forum.Controllers.Forum
{
	public class ThreadController : Controller
	{
		public ActionResult Thread(string threadName = null, int pageIndex = -1)
		{
			var model = new ThreadModel(GetMessages(), threadName, pageIndex);
			return View(model);
		}

		private ForumMessage[] GetMessages()
		{
			return new[]
			{
				new ForumMessage("Hi all!", "deniaa", DateTime.Now),
				new ForumMessage(@"What's a lovely day!
What's a lovely day!
What's a lovely day!
What's a lovely day!
What's a lovely day!
What's a lovely day!
What's a lovely day!
What's a lovely day!
What's a lovely day!
What's a lovely day!
What's a lovely day!
What's a lovely day!", "deniaa", DateTime.Now)
			};
		}


		public ActionResult SendMessage(string returnUrl = null, string threadName = null, int pageIndex = -1, string message = null)
		{
			var messages = GetMessages();
			var userName = User.Identity.Name ?? "Unknown user";
			if (userName == "")
				userName = "Unknown user";
			messages = messages.Concat(new[]{new ForumMessage(message, userName, DateTime.Now)}).ToArray();
			var model = new ThreadModel(messages, threadName, pageIndex);
			return View("~/Views/Thread/Thread.cshtml", model);
		}
	}
}