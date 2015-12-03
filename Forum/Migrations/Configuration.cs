using System.Collections.Generic;
using System.Web.Security;
using Forum.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Forum.Migrations
{
	using System;
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using System.Linq;

	internal sealed class Configuration : DbMigrationsConfiguration<ForumDatabase>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = false;
		}

		protected override void Seed(ForumDatabase context)
		{
			var roleStore = new RoleStore<IdentityRole>(context);
			var roleManager = new RoleManager<IdentityRole>(roleStore);
			roleManager.Create(new IdentityRole(Role.Admin.ToString()));
			roleManager.Create(new IdentityRole(Role.User.ToString()));
			if (!context.Users.Any(u => u.UserName == "user"))
			{
				var userStore = new UserStore<ApplicationUser>(context);
				var manager = new UserManager<ApplicationUser>(userStore);
				var user = new ApplicationUser { UserName = "user", EmailConfirmed = true };
				manager.Create(user, "asdasd");
			}
			if (!context.Users.Any(u => u.UserName == "Deniaa"))
			{
				var userStore = new UserStore<ApplicationUser>(context);
				var manager = new UserManager<ApplicationUser>(userStore);
				var user = new ApplicationUser { UserName = "Deniaa", EmailConfirmed = true, Email = "silentcatvallis@gmail.com" };
				manager.Create(user, "fullcontrol");
				manager.AddToRoleAsync(user.Id, Role.Admin.ToString()).Wait();
			}

			var threads = new[]
	        {
		        new ForumThread
		        {
			        ThreadName = "C#",
			        Themes = new[]
			        {
				        new Theme
				        {
					        Name = "Task sheduler",
							Messages = new List<ForumMessage>()
				        },
				        new Theme
				        {
					        Name = "Reflection",
							Messages = new List<ForumMessage>()
				        }
						
			        }
		        },
		        new ForumThread
		        {
			        ThreadName = "C++",
			        Themes = new[]
			        {
				        new Theme
				        {
					        Name = "Как прострелить себе ногу",
							Messages = new List<ForumMessage>()
				        },
				        new Theme
				        {
					        Name = "Ничего не работает, помогите",
							Messages = new List<ForumMessage>()
				        }
			        }
		        }
	        };

			if (!context.Threads.Select(th => th.ThreadName).Contains("C#"))
				context.Threads.AddRange(threads);

			context.SaveChanges();
		}
	}
}
