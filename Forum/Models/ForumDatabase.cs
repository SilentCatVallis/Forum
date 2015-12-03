using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Services.Description;
using Forum.Forum;
using Forum.Migrations;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Forum.Models
{
	using System;
	using System.Data.Entity;
	using System.Linq;

	public class ForumDatabase : IdentityDbContext<ApplicationUser>
	{
		// Your context has been configured to use a 'ForumDatabase' connection string from your application's 
		// configuration file (App.config or Web.config). By default, this connection string targets the 
		// 'Forum.Models.ForumDatabase' database on your LocalDb instance. 
		// 
		// If you wish to target a different database and/or database provider, modify the 'ForumDatabase' 
		// connection string in the application configuration file.
		public ForumDatabase()
			: base("ForumDatabase", throwIfV1Schema: false)
		{
			Database.SetInitializer(new MigrateDatabaseToLatestVersion<ForumDatabase, Configuration>());
		}

		// Add a DbSet for each entity type that you want to include in your model. For more information 
		// on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

		//public virtual DbSet<User> Users { get; set; }

		public virtual DbSet<ForumThread> Threads { get; set; }
	}

	public class ForumThread
	{
		[Required]
		public int Id { get; set; }

		[Required]
		[StringLength(30)]
		public string ThreadName { get; set; }

		[Required]
		public virtual ICollection<Theme> Themes { get; set; }
	}

	public class Theme
	{
		[Required]
		public int Id { get; set; }

		[Required]
		[StringLength(50)]
		public string Name { get; set; }

		[Required]
		public virtual ICollection<ForumMessage> Messages { get; set; }
	}

	public class ForumMessage
	{
		[Required]
		public int Id { get; set; }

		[Required]
		[StringLength(500)]
		public string Text { get; set; }

		[Required]
		public string  UserId { get; set; }

		[Required]
		public DateTime Time { get; set; }
	}

	//public class User
	//{
	//	[Required]
	//	public int Id { get; set; }
	//
	//	[Required]
	//	[StringLength(50)]
	//	public string Name { get; set; }
	//
	//	public Role Role { get; set; }
	//}

	public enum Role
	{
		User,
		Admin
	}
}