using static Xamarin.Essentials.Permissions;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using MobileApp.Views.AddViews;
using MobileApp.Views.ModifyViews;

namespace MobileApp.ViewModels
{
    class AddUserViewModel : BaseViewModel
    {
        public Command SaveCommand { get; }
        public Command ModifyUserCommand { get; }
        public Command CancelCommand { get; }
        private int student_index;
        private string username;
        private string password;
        private List<Models.Student> studentslist;
        private List<string> studentnames;

        public int StudentIndex
        {
            get => student_index; set => SetProperty(ref student_index, value);
        }

        public List<string> StudentNames
        {
            get => studentnames; set => SetProperty(ref studentnames, value);
        }

        public List<Models.Student> StudentList
        {
            get => studentslist; set => SetProperty(ref studentslist, value);
        }

        public string Password
        {
            get => password; set => SetProperty(ref password, value);
        }

        public string Username
        {
            get => username; set => SetProperty(ref username, value);
        }

        public AddUserViewModel()
        {
            SaveCommand = new Command(OnSave);
            ModifyUserCommand = new Command(GotoModifyUser);
            CancelCommand = new Command(OnCancel);
            StudentList = App.DataBase.GetAllStudents().Result;
            StudentNames = GetStudentNames(StudentList);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private List<string> GetStudentNames(List<Models.Student> studentlist)
        {
            List<string> nameslist = new List<string>();
            foreach (var student in studentlist)
            {
                nameslist.Add(student.Name);
            }
            return nameslist;
        }

        private int GetStudentID(int studentindex)
        {
            int id = StudentList[studentindex].ID;
            return id;
        }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            Models.User user = new Models.User()
            {
                Username = Username,
                Password = Password,
                StudentID = GetStudentID(StudentIndex),
            };

            await App.DataBase.SaveUser(user);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void GotoModifyUser()
        {
            await Shell.Current.GoToAsync($"{nameof(ModifyUser)}");
        }
    }
}