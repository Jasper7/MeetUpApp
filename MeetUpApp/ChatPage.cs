using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace MeetUpApp
{
	public class ChatPage : ContentPage
	{
		public ListView ChatList;
		public ChatPage(ObservableCollection<Message> source)
		{
			Title = "Chat";

			ChatList = new ListView();

			var template = new DataTemplate(typeof(TextCell));
			template.SetBinding(TextCell.TextProperty, "UserName");
			template.SetBinding(TextCell.DetailProperty, "Text");
			ChatList.ItemTemplate = template;

			ChatList.ItemsSource = source;


			var edit = new Entry()
			{
				Placeholder = "Сообщение",
				WidthRequest = 100
			};

			var btn = new Button
			{
				Text = "Отправить",
				WidthRequest = 100
			};

			btn.Clicked += async delegate
			{

				var msg = new Message(edit.Text, App.DeviceName);
				source.Add(msg);

				await App.Hub.Invoke("SendMessage", msg);
				edit.Text = String.Empty;
			};


			Content = new StackLayout
			{
				Padding = 20,
				Spacing = 30,
				VerticalOptions = LayoutOptions.Center,
				Children = {
						ChatList,
					edit,
					btn
				}
			};
		}
	}
}