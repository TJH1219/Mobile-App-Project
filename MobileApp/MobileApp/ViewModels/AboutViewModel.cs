using MobileApp.Models;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Collections.Generic;
using MobileApp.Views.ModifyViews;
using MobileApp.Views.AddViews;
using MobileApp.Views;

namespace MobileApp.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        private Models.Instructor instructor;
        private Models.Student student;
        private List<Student> students;
        private List<Instructor> instructors;
        private string studentname;
        private string studentemail;
        private string studentphone;
        private string instructorname;
        private string instructorphone;
        private string instructoremail;
        public Command CreateUserCommand { get; }
        public Command ModifyUserCommand { get; }
        public Command CreateStudentCommand { get; }
        public Command ModifyStudentCommand { get; }
        public Command ClearNotificationsCommand { get; set; }

        public string InstructorPhone
        {
            get => instructorphone; set => SetProperty(ref instructorphone, value);
        }

        public string InstructorEmail
        {
            get => instructoremail; set => SetProperty(ref instructoremail, value);
        }

        public string InstructorName
        {
            get => instructorname; set => SetProperty(ref instructorname, value);
        }

        public string StudentPhone
        {
            get => studentphone; set => SetProperty(ref studentphone, value);
        }

        public string StudentEmail
        {
            get => studentemail; set => SetProperty(ref studentemail, value);
        }

        public string StudentName
        {
            get => studentname; set => SetProperty(ref studentname, value);
        }

        public Models.Student Student
        {
            get => student; set => SetProperty(ref student, value);
        }

        public List<Instructor> Instructors
        {
            get => instructors; set => SetProperty(ref instructors, value);
        }

        public List<Student> StudentList
        {
            get => students; set => SetProperty(ref students, value);
        }

        public Models.Instructor Instructor
        {
            get => instructor; set => SetProperty(ref instructor, value);
        }

        public AboutViewModel()
        {
            Title = "Home";
            CreateUserCommand = new Command(CreateUser);
            ModifyUserCommand = new Command(GotoModifyUser);
            CreateStudentCommand = new Command(CreateStudent);
            ModifyStudentCommand = new Command(GotoModifyStudent);
            ClearNotificationsCommand = new Command(ClearNotifications);
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamarin-quickstart"));
            StudentList = App.DataBase.GetAllStudents().Result;
            Instructors = App.DataBase.GetAllInstructors().Result;
            try
            {
                Student = GetStudent(0);
                SetStudentInfo();
            }
            catch (Exception ex)
            {
                Student = null;
            }
            try
            {
                Instructor = instructors[Student.InstructorID - 1];
                SetInstuctorinfo();
            }
            catch (Exception ex)
            {
                Instructor = null;
            }
        }

        public ICommand OpenWebCommand { get; }

        private Models.Student GetStudent(int index)
        {
            return StudentList[index];
        }

        private void SetStudentInfo()
        {
            StudentName = Student.Name;
            StudentEmail = Student.Email;
            StudentPhone = Student.Phone;
        }

        private void SetInstuctorinfo()
        {
            InstructorName = Instructor.Name;
            InstructorEmail = Instructor.Email;
            InstructorPhone = Instructor.Phone;
        }

        private async void ClearNotifications()
        {
            List<Models.Reminders> list = await App.DataBase.GetAllReminder();
            foreach (Models.Reminders reminder in list)
                await App.DataBase.DeleteReminder(reminder);
        }

        private async void GotoModifyUser()
        {
            await Shell.Current.GoToAsync($"{nameof(ModifyUser)}");
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
    }
}