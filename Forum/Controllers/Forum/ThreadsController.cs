using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Forum.Models.Forum;

namespace Forum.Controllers.Forum
{
	public class ThreadsController : Controller
	{
		public ActionResult Threads()
		{
			return View(new ThreadsModel(new []
			{
				"JS",
				"C++",
				"C#",
				"Python"
			}));
		}
	}
}