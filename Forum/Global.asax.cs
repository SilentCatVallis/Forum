using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Forum.Models;

namespace Forum
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

			//Database.SetInitializer(new ForumDbInitializer());
        }
    }

	//public class ForumDbInitializer : IDatabaseInitializer<ForumDatabase>
	//{
	//	public void InitializeDatabase(ForumDatabase context)
	//	{
	//		var exists = context.Database.Exists();
	//
	//		try
	//		{
	//			if (exists && context.Database.CompatibleWithModel(true))
	//			{
	//				// everything is good , we are done
	//				return;
	//			}
	//
	//			if (!exists)
	//			{
	//				context.Database.Create();
	//			}
	//		}
	//		catch (Exception)
	//		{
	//			//Something is wrong , either we could not locate the metadata or the model is not compatible.
	//			if (exists)
	//			{
	//				context.Database..ExecuteSqlCommand("ALTER DATABASE Pariksha SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
	//				context.Database.ExecuteSqlCommand("USE Master DROP DATABASE Pariksha");
	//				context.SaveChanges();
	//			}
	//
	//			context.Database.Create();
	//		}
	//	}
	//}
}
