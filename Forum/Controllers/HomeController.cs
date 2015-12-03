using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Forum.Models;
using Forum.Models.Forum;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace Forum.Controllers
{
	public class HomeController : Controller
	{
		private readonly ForumDatabase ForumDatabase = new ForumDatabase();
		public UserManager<ApplicationUser> UserManager { get; private set; }

		public HomeController()
			: this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ForumDatabase())))
		{
			ForumDatabase = new ForumDatabase();
			UserManager.UserValidator =
				new UserValidator<ApplicationUser>(UserManager)
				{
					AllowOnlyAlphanumericUserNames = false
				};
		}

		public HomeController(UserManager<ApplicationUser> userManager)
		{
			UserManager = userManager;
		}

		public ActionResult Index()
		{
			var threads = GetThreads();
			var isAdmin = User.Identity.IsAuthenticated && User.IsInRole(Role.Admin.ToString());
			return View(new ThreadsModel(threads, isAdmin));
		}

		public ActionResult CreateTheme(string threadName, string returnUrl, string themeName)
		{
			try
			{
				if (ForumDatabase
					.Threads
					.First(th => th.ThreadName == threadName)
					.Themes
					.Any(th => th.Name == themeName))
				{
					return Json(new
					{
						Result = false,
						Message = string.Format("Theme with name {0} exist", themeName)
					});
				}
				SaveNewTheme(threadName, themeName);
				return Json(new
				{
					Result = true
				});
			}
			catch (Exception)
			{
				return Json(new
				{
					Result = false,
					Message = "erorr"
				});
			}
		}

		private void SaveNewTheme(string threadName, string themeName)
		{
			ForumDatabase
				.Threads
				.First(th => th.ThreadName == threadName)
				.Themes
				.Add(new Theme
				{
					Messages = new List<ForumMessage>(),
					Name = themeName
				});
			ForumDatabase.SaveChangesAsync();
		}

		public ActionResult CreateThread(string threadName, string returnUrl = null)
		{
			try
			{
				if (ForumDatabase
					.Threads
					.Any(th => th.ThreadName == threadName))
				{
					return Json(new
					{
						Result = false,
						Message = string.Format("Thread with name {0} exist", threadName)
					});
				}
				SaveNewThread(threadName);
				return Json(new
				{
					Result = true
				});
			}
			catch (Exception)
			{
				return Json(new
				{
					Result = false,
					Message = "erorr"
				});
			}
		}

		private void SaveNewThread(string threadName)
		{
			ForumDatabase
				.Threads
				.Add(new ForumThread
				{
					Themes = new List<Theme>(),
					ThreadName = threadName
				});
			ForumDatabase.SaveChangesAsync();
		}

		private Dictionary<string, string[]> GetThreads()
		{
			var dict = new Dictionary<string, string[]>();
			foreach (var threadName in ForumDatabase.Threads.Select(x => x.ThreadName).ToList())
			{
				dict[threadName] = ForumDatabase.Threads
					.First(th => th.ThreadName == threadName)
					.Themes
					.Select(t => t.Name)
					.ToArray();
			}
			return dict;
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}

		[Authorize]
		public ActionResult Users()
		{
			if (User.IsInRole(Role.Admin.ToString()))
				return View(new UsersModel(
					UserManager.Users.Select(u => u.UserName),
					new HashSet<string>(UserManager.Users.ToList()
						.Where(u => UserManager.IsInRole(u.Id, Role.Admin.ToString()))
						.Select(u => u.UserName)
						.ToList()), true));
			return View(new UsersModel(new[]{User.Identity.Name}, new HashSet<string>(), false));
		}


		[HttpPost]
		[Authorize]
		public async Task<ActionResult> DeleteUser(string userName)
		{
			try
			{
				if (User.IsInRole(Role.Admin.ToString()))
				{
					var userForDelete = ForumDatabase.Users.FirstOrDefault(u => u.UserName == userName);
					if (userForDelete != null)
					{
						ForumDatabase.Users.Remove(userForDelete);
						await ForumDatabase.SaveChangesAsync();
						return Json(new
						{
							Result = true
						});
					}
				}
				throw new Exception();
			}
			catch
			{
				return Json(new
				{
					Result = false
				});
			}
		}
		public void ToggleRole(string userId, string role)
		{
			if (UserManager.IsInRole(userId, role))
				UserManager.RemoveFromRole(userId, role);
			else
				UserManager.AddToRole(userId, role);
		}

		[HttpPost]
		[Authorize]
		public ActionResult DisableAdmin(string userName)
		{
			try
			{
				if (User.IsInRole(Role.Admin.ToString()))
				{
					var user = ForumDatabase.Users.FirstOrDefault(u => u.UserName == userName);
					if (user != null)
					{
						ToggleRole(user.Id, Role.Admin.ToString());
						return Json(new
						{
							Result = true
						});
					}
				}
				throw new Exception();
			}
			catch
			{
				return Json(new
				{
					Result = false
				});
			}
		}

		[HttpPost]
		[Authorize]
		public ActionResult EnableAdmin(string userName)
		{
			try
			{
				if (User.IsInRole(Role.Admin.ToString()))
				{
					var user = ForumDatabase.Users.FirstOrDefault(u => u.UserName == userName);
					if (user != null)
					{
						ToggleRole(user.Id, Role.Admin.ToString());
						return Json(new
						{
							Result = true
						});
					}
				}
				throw new Exception();
			}
			catch
			{
				return Json(new
				{
					Result = false
				});
			}
		}

		[HttpPost]
		[Authorize]
		public async Task<ActionResult> DeleteThread(string threadName)
		{
			try
			{
				if (User.IsInRole(Role.Admin.ToString()))
				{
					var thread = ForumDatabase.Threads.FirstOrDefault(th => th.ThreadName == threadName);
					if (thread != null)
					{
						ForumDatabase.Threads.Remove(thread);
						await ForumDatabase.SaveChangesAsync();
						return Json(new
						{
							Result = true
						});
					}
				}
				throw new Exception();
			}
			catch
			{
				return Json(new
				{
					Result = false
				});
			}
		}

		[HttpPost]
		[Authorize]
		public async Task<ActionResult> DeleteTheme(string threadName, string themeName)
		{
			try
			{
				if (User.IsInRole(Role.Admin.ToString()))
				{
					var thread = ForumDatabase.Threads.FirstOrDefault(th => th.ThreadName == threadName);
					if (thread != null)
					{
						var theme = thread.Themes.FirstOrDefault(th => th.Name == themeName);
						if (theme != null)
						{
							thread.Themes.Remove(theme);
							await ForumDatabase.SaveChangesAsync();
							return Json(new
							{
								Result = true
							});
						}
					}
				}
				throw new Exception();
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
}