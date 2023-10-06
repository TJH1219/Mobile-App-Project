using static Xamarin.Essentials.Permissions;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using MobileApp.Views.AddViews;
using MobileApp.Views.ModifyViews;
using System.Runtime.InteropServices.ComTypes;

namespace MobileApp.ViewModels
{
    class ModifyUserViewModel : BaseViewModel
    {
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        public Command DeleteCommand { get; }

        private int studentID;
        private int student_index;
        private string username;
        private string password;
        private List<string> usernamelist;
        private List<Models.User> userslist;
        private List<Models.Student> studentslist;
        private int selecteduserindex;
        private List<string> studentnames;

        public int StudentID
        {
            get => studentID; set => SetProperty(ref studentID, value);
        }

        //index of the selected user
        public int SelectedUserIndex
        {
            get => selecteduserindex;
            set
            {
                if(selecteduserindex != value)
                {
                    selecteduserindex = value;
                    Models.User user = Userslist[value];
                    Username = user.Username;
                    Password = user.Password;
                    StudentID = user.StudentID;
                }
            }
        }

        public List<string> UserNameList
        {
            get => usernamelist; set => SetProperty(ref usernamelist, value);
        }

        public List<Models.User> Userslist
        {
            get => userslist; set => SetProperty(ref userslist, value);
        }

        // index of the currently selected student
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

        public ModifyUserViewModel()
        {
            SaveCommand = new Command(OnSave);
            CancelCommand = new Command(OnCancel);
            DeleteCommand = new Command(onDelete);
            StudentList = App.DataBase.GetAllStudents().Result;
            StudentNames = GetStudentNames(StudentList);
            Userslist = App.DataBase.GetAllUsers().Result;
            UserNameList = GetUserNames(Userslist);
            SelectedUserIndex = 0;
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        //private int GetStudentIndex(int id)
        //{
            //int index = -1;
            //foreach(var user in StudentList)
            //{
              //  int i = 0;

            //}
        //}

        private List<string> GetUserNames(List<Models.User> userlist)
        {
            List<string> namelist = new List<string>();
            foreach (Models.User user in userlist)
            {
                namelist.Add(user.Username);
            }
            return namelist;
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
                StudentID = 0,//GetStudentID(StudentIndex),
            };

            await App.DataBase.UpdateUser(user);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void onDelete()
        {
            Models.User user = Userslist[SelectedUserIndex];
            await App.DataBase.DeleteUser(user);
        }
    }
}