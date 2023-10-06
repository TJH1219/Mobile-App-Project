using Plugin.LocalNotifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AssessmentDetailView : ContentPage
    {
        public AssessmentDetailView()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

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
    }
}