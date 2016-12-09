using System;

using Xamarin.Forms;

namespace MeetUpApp
{
	public class Message 
	{
		public string UserName { get; set; }
		public string Text { get; set; }

		public Message(string text, string userName)
		{
			UserName = userName;
			Text = text;
		}
	}
}

