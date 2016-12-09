using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Xamarin.Forms;

namespace MeetUpApp
{
	public class App : Application
	{
		public static HubConnection Connection;
		public static IHubProxy Hub;

		private Label TextLabel;
		public static string DeviceName = String.Empty;

		private bool ValueFromServer = false;

		private  ObservableCollection<Message> MessageList = new ObservableCollection<Message>();

		private ChatPage chatPage;

		public App()
		{
			Device.OnPlatform(
				() => DeviceName = "iOS", 
				() => DeviceName = "Android");

			TextLabel = new Label()
			{
				HorizontalTextAlignment = TextAlignment.Center,
				Text = "Привет здесь будет магия"
			};

			var entry = new Entry
			{
				HorizontalTextAlignment = TextAlignment.Center
			};

			entry.TextChanged += async (object sender, TextChangedEventArgs e) =>
			{
				await Hub.Invoke("Send", entry.Text);
			};

			// The root page of your application
			var content = new ContentPage
			{
				Title = "Text",
				Content = new StackLayout
				{
					Padding = 20,
					Spacing = 30,
					VerticalOptions = LayoutOptions.Center,
					Children = {
						TextLabel,
						entry
						//MySlider
					}
				}
			};

			chatPage = new ChatPage(MessageList);

			var tabPage = new TabbedPage();
			tabPage.Children.Add(content);
			tabPage.Children.Add(chatPage);
			MainPage = tabPage;
		}

		protected async override void OnStart()
		{
			//Указываем адрес нашего SignalR сервера
			Connection = new HubConnection("http://meetupappxamarin.azurewebsites.net/signalr");

			//Пишем имя нашего хаба
			Hub = Connection.CreateHubProxy("AppHub");

			Hub.On<string>("Recive", message =>
			{
				Device.BeginInvokeOnMainThread(() => TextLabel.Text = message);
			});

			Hub.On<Message>("ReciveMessage", message =>
			{
				Device.BeginInvokeOnMainThread(() => { 
					MessageList.Add(message);
				});
			});

			Connection.StateChanged += (StateChange obj) => {
				Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
				{
					if (obj.NewState == ConnectionState.Connected)
						Current.MainPage.Title = "Подключено";
					else
						if (obj.NewState == ConnectionState.Disconnected)
						Current.MainPage.Title = "Отключено";
				});
				
			};

			await Connection.Start();

		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
