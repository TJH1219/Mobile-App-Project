using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using static Xamarin.Essentials.Permissions;
using Xamarin.Essentials;
using Xamarin.Forms;
using MobileApp.Models;

namespace MobileApp.ViewModels
{
    internal class ModifyInstructorViewModel : BaseViewModel
    {
        private string name;
        private string email;
        private string phone;
        private string photopath;
        private List<Models.Instructor> instructors;
        private List<string> instructnames;
        private int instuctindex;

        public int InstuctIndex
        {
            get => instuctindex; set
            {
                if (instuctindex != value)
                {
                    SetProperty(ref instuctindex, value);
                    Models.Instructor instruct = Instructors[value];
                    Name = instruct.Name;
                    Email = instruct.Email;
                    Phone = instruct.Phone;
                }
            }
        }

        public ModifyInstructorViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            DeleteCommand = new Command(DeleteInstructor);
            Instructors = App.DataBase.GetAllInstructors().Result;
            InstructNames = GetInstructorNames(Instructors);
            PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }


        public List<string> InstructNames
        {
            get => instructnames; set => SetProperty(ref instructnames, value);
        }

        public List<Models.Instructor> Instructors
        {
            get => instructors; set => SetProperty(ref instructors, value);
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
        public Command DeleteCommand { get; }

        private List<string> GetInstructorNames(List<Models.Instructor> instructlist)
        {
            List<string> nameslist = new List<string>();
            foreach(Models.Instructor instructor in instructlist) 
            {
                nameslist.Add(instructor.Name);
            }
            return nameslist;
        }

        private bool ValidateSave()
        {
            return !string.IsNullOrWhiteSpace(name)
                && !string.IsNullOrWhiteSpace(email)
                && !string.IsNullOrEmpty(phone);
        }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private int GetInstructorID(int instuctorindex)
        {
            int id = Instructors[instuctorindex].ID;
            return id;
        }

        private async void DeleteInstructor()
        {
            await App.DataBase.DeleteInstructor(Instructors[InstuctIndex]);
            await Shell.Current.GoToAsync($"..\\..\\..");
        }

        private async void OnSave()
        {
            Models.Instructor instruct = new Models.Instructor()
            {
                ID = GetInstructorID(InstuctIndex),
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
