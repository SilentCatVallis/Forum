using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum.Forum
{
	public class ForumMessage
	{
		public ForumMessage(string text, string author, DateTime time)
		{
			Text = text;
			Author = author;
			Time = time;
		}

		public string Text { get; private set; }
		public string Author { get; private set; }
		public DateTime Time { get; private set; }
	}
}