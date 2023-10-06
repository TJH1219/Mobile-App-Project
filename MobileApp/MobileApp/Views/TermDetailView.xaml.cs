using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileApp.ViewModels;
using Plugin.LocalNotifications;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TermDetailView : ContentPage
	{
        public TermDetailsViewModel _viewmodel { get; set; }

		public TermDetailView ()
		{
			InitializeComponent();
            BindingContext = _viewmodel = new TermDetailsViewModel();
        }

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			_viewmodel.OnAppearing();

			var reminderlist = await App.DataBase.GetAllReminder();
			var notifyrandom = new Random();
			var notifyid = notifyrandom.Next(1000);
			DateTime test = DateTime.Today;

			foreach(var reminder in reminderlist)
			{
				if (reminder.TriggerDate == DateTime.Today)
				{
					CrossLocalNotifications.Current.Show("Notice", $"{reminder.Name} begins today", notifyid);
				}
			}
		}
	}
}