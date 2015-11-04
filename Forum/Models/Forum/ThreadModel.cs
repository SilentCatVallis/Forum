using Forum.Forum;

namespace Forum.Models.Forum
{
	public class ThreadModel
	{
		public ThreadModel(ForumMessage[] messages, string threadName, int pageIndex)
		{
			Messages = messages;
			ThreadName = threadName;
			PageIndex = pageIndex;
		}

		public int PageIndex { get; private set; }

		public string ThreadName { get; private set; }
		public ForumMessage[] Messages { get; private set; }	
	}
}