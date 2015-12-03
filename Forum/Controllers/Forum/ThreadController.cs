using System;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Forum.Models;
using Forum.Models.Forum;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace Forum.Controllers.Forum
{
	public class ThreadController : Controller
	{
		private readonly ForumDatabase ForumDatabase = new ForumDatabase();
		public UserManager<ApplicationUser> UserManager { get; private set; }

		public ThreadController()
			: this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ForumDatabase())))
		{
			ForumDatabase = new ForumDatabase();
			UserManager.UserValidator =
				new UserValidator<ApplicationUser>(UserManager)
				{
					AllowOnlyAlphanumericUserNames = false
				};
		}

		public ThreadController(UserManager<ApplicationUser> userManager)
		{
			UserManager = userManager;
		}




		public ActionResult Thread(string threadName = null, string themeName = null, int pageIndex = 0)
		{
			var messages = GetMessages(threadName, themeName, ref pageIndex);
			var userName = User.Identity.IsAuthenticated 
				? User.Identity.Name 
				: "";
			var isAdmin = User.Identity.IsAuthenticated && User.IsInRole(Role.Admin.ToString()) ;
			var model = new ThreadModel(messages , threadName, themeName, pageIndex, userName, isAdmin);
			return View(model);
		}

		private ThemeMessage[] GetMessages(string threadName, string themeName, ref int pageIndex)
		{
			const int take = 10;
			try
			{
				var messageCount = ForumDatabase.Threads
					.First(th => th.ThreadName == threadName)
					.Themes
					.First(theme => theme.Name == themeName)
					.Messages
					.Count;
				var oldIndex = pageIndex;
				pageIndex--;
				var newIndex = pageIndex;
				if (newIndex == -1)
					newIndex = messageCount / 10;
				var messagesSkip = newIndex * 10;
				if (messageCount%10 != 0 && pageIndex == -1)
					newIndex++;
				pageIndex = newIndex;
				pageIndex++;
				if (oldIndex == 0)
					pageIndex--;
				return ForumDatabase.Threads
					.First(th => th.ThreadName == threadName)
					.Themes
					.First(theme => theme.Name == themeName)
					.Messages
					.Skip(messagesSkip)
					.Take(take)
					.Select(m => new ThemeMessage(m.Text, GetAuthorName(m), m.Time))
					.ToArray();
			}
			catch
			{
				return new ThemeMessage[0];
			}
		}

		private string GetAuthorName(ForumMessage m)
		{
			var user = UserManager.Users.FirstOrDefault(u => u.Id == m.UserId);
			return user != null ? user.UserName : "-";
		}

		[HttpPost]
		[Authorize]
		public ActionResult SendMessage(string returnUrl = null, string threadName = null, string themeName = null, int pageIndex = -1, string message = null)
		{
			try
			{
				var time = DateTime.Now;
				if (message != null)
				{
					ForumDatabase
						.Threads
						.First(th => th.ThreadName == threadName)
						.Themes
						.First(th => th.Name == themeName)
						.Messages
						.Add(new ForumMessage
						{
							UserId = User.Identity.GetUserId(),
							Text = message,
							Time = time
						});
					ForumDatabase.SaveChangesAsync();
					return Json(new
					{
						Time = time.ToString(CultureInfo.CurrentCulture),
						Name = User.Identity.Name,
						Text = message
					});
				}
			}
			catch
			{
				return Json(false);
			}
			return Json(false);
		}

		[HttpPost]
		[Authorize]
		public ActionResult DeleteMessage(int messageIndex, string threadName, string themeName, int pageIndex)
		{
			try
			{
				var message = ForumDatabase
					.Threads
					.First(th => th.ThreadName == threadName)
					.Themes
					.First(th => th.Name == themeName)
					.Messages
					.Skip((pageIndex - 1)*10 + messageIndex)
					.First();
				ForumDatabase
					.Threads
					.First(th => th.ThreadName == threadName)
					.Themes
					.First(th => th.Name == themeName)
					.Messages
					.Remove(message);
				ForumDatabase.SaveChangesAsync();
				return Json(new
				{
					Result = true
				});
			}
			catch
			{
				return Json(new
				{
					Result = false
				});
			}
		}
	}

	public class ThemeMessage
	{
		public ThemeMessage(string message, string user, DateTime dateTime)
		{
			Text = message;
			Author = user;
			Time = dateTime;
		}

		public string Author { get; private set; }
		public string Text { get; private set; }
		public DateTime Time { get; private set; }
	}
}