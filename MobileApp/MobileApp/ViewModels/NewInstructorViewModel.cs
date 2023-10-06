using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Xamarin.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace MobileApp.ViewModels
{
    class NewInstructorViewModel : BaseViewModel
    {
        private string name;
        private string email;
        private string phone;
        private string photopath;

        public NewInstructorViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        public string Name
        {
            get { return name; }
            set => SetProperty(ref name, value);
        }

        public string Email
        {
            get { return email; }
            set => SetProperty(ref email, value);
        }

        public string Phone
        {
            get { return phone; }
            set => SetProperty(ref phone, value);
        }

        public string Photopath
        {
            get { return photopath; }
            set => SetProperty(ref photopath, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private bool ValidateSave()
        {
            return !string.IsNullOrWhiteSpace(name)
                && !string.IsNullOrWhiteSpace(email)
                && !string.IsNullOrEmpty(phone);
        }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync($"..\\..\\..");
        }

        private async void OnSave()
        {
            Models.Instructor instruct = new Models.Instructor()
            {
                Name = Name,
                Email = Email,
                Phone = Phone,
            };

            await App.DataBase.SaveInstructor(instruct);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync($"..\\..\\..");
        }
    }
}