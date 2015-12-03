using System.Net;
using Forum.Controllers.Forum;
using Forum.Forum;

namespace Forum.Models.Forum
{
	public class ThreadModel
	{
		public ThreadModel(ThemeMessage[] messages, string threadName, string themeName, int pageIndex, string author, bool isAdmin)
		{
			IsAdmin = isAdmin;
			Author = author;
			Messages = messages;
			ThreadName = threadName;
			PageIndex = pageIndex;
			ThemeName = themeName;
		}

		public bool IsAdmin { get; set; }

		public string Author { get; set; }

		public string ThemeName { get; private set; }

		public int PageIndex { get; private set; }

		public string ThreadName { get; private set; }

		public ThemeMessage[] Messages { get; private set; }
	}
}