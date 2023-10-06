using MobileApp.Views;
using MobileApp.Views.AddViews;
using MobileApp.Views.ModifyViews;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }
        public Command CreateUserCommand { get; }
        public Command ModifyUserCommand { get; }
        public Command CreateStudentCommand { get; }
        public Command ModifyStudentCommand { get; }

        private string username;
        private string password;

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            CreateUserCommand = new Command(CreateUser);
            ModifyUserCommand = new Command(GotoModifyUser);
            CreateStudentCommand = new Command(CreateStudent);
            ModifyStudentCommand = new Command(GotoModifyStudent);
        }

        public string UserName
        {
            get => username; set => SetProperty(ref username, value);
        }

        public string Password
        {
            get => password; set => SetProperty(ref password, value);
        }

        private async Task<int> CheckPassword(string password)
        {
            int id = -1;
            List<Models.User> userlist = await App.DataBase.GetAllUsers();
            foreach (Models.User user in userlist)
            {
                if(user.Password == password)
                {
                    id = user.ID;
                }
            }
            return id;
        }

        private async Task<Models.User> GetUser(int userid)
        {
            Models.User User = new Models.User();
            List<Models.User> userlist = await App.DataBase.GetAllUsers();
            foreach(Models.User user in userlist)
            {
                if(user.ID == userid)
                {
                    User = user;
                }
            }
            return User;
        }

        private async void CreateUser()
        {
            await Shell.Current.GoToAsync($"{nameof(AddUser)}");
        }

        private async void CreateStudent()
        {
            await Shell.Current.GoToAsync($"{nameof(AddNewStudent)}");
        }

        private async void GotoModifyStudent()
        {
            await Shell.Current.GoToAsync($"{nameof(ModifyStudent)}");
        }

        private async void OnLoginClicked(object obj)
        {
            //int id = await CheckPassword(Password);
            //if (id == -1)
            //{
            //  return;
            //}
            //App.User = await GetUser(id);

            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync("//AboutPage");
        }

        private async void GotoModifyUser()
        {
            await Shell.Current.GoToAsync($"{nameof(ModifyUser)}");
        }
    }
}
