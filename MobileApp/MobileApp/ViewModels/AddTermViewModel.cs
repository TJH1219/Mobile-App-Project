using System;
using System.Collections.Generic;
using System.Text;
using static Xamarin.Essentials.Permissions;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Runtime.CompilerServices;

namespace MobileApp.ViewModels
{
    class AddTermViewModel : BaseViewModel
    {
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        private DateTime startdate;
        private DateTime enddate;
        private string name;

        public string Name
        {
            get => name; set => SetProperty(ref name, value);
        }

        public DateTime StartDate
        {
            get => startdate; set => SetProperty(ref startdate, value);
        }

        public DateTime EndDate
        {
            get => enddate; set => SetProperty(ref enddate, value);
        }

        public AddTermViewModel()
        {
            StartDate = DateTime.Today;
            EndDate = DateTime.Today;
            SaveCommand = new Command(OnSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }


        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            Models.Term term = new Models.Term()
            {
                Name = Name,
                StartDate = StartDate,
                EndDate = EndDate
            };

            await App.DataBase.SaveTerm(term);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
