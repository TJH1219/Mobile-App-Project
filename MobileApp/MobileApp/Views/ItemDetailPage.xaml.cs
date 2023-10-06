using MobileApp.ViewModels;
using Plugin.LocalNotifications;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace MobileApp.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailViewModel _viewModel { get; set; }

        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new ItemDetailViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();

            var reminderlist = await App.DataBase.GetAllReminder();
            var notifyrandom = new Random();
            var notifyid = notifyrandom.Next(1000);

            foreach (var reminder in reminderlist)
            {
                if (reminder.TriggerDate == DateTime.Today)
                {
                    CrossLocalNotifications.Current.Show("Notice", $"{reminder.Name} begins today", notifyid);
                }
            }
        }

        private void Button_Clicked(object sender, System.EventArgs e)
        {

        }
    }
}