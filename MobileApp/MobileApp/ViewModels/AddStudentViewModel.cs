using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    internal class AddStudentViewModel : BaseViewModel
    {
        private List<string> termnames;
        private List<string> instructnames;
        private List<string> coursenames;
        private List<Models.Instructor> instructors;
        private List<Models.Term> terms;
        private List<Models.Course> courses;
        private string name;
        private string email;
        private string phone;
        private int instructorindex;
        private int termindex;
        private int courseindex;

        public AddStudentViewModel()
        {
            Instructors = App.DataBase.GetAllInstructors().Result;
            Terms = App.DataBase.GetAllTerms().Result;
            Courses = App.DataBase.GetAllCourses().Result;
            TermNames = GetTermNames(Terms);
            InstructNames = GetInstructorNames(Instructors);
            CourseNames = GetCourseNames(Courses);
            SaveCommand = new Command(OnSave);
            CancelCommand = new Command(OnCancel);
            PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        public int InstructorIndex
        {
            get => instructorindex; set => SetProperty(ref instructorindex, value);
        }

        public int TermIndex
        {
            get => termindex; set => SetProperty(ref termindex, value);
        }

        public int CourseIndex
        {
            get => courseindex; set => SetProperty(ref courseindex, value);
        }

        public List<string> TermNames
        {
            get => termnames; set => SetProperty(ref termnames, value);
        }

        public List<string> CourseNames
        {
            get => coursenames; set => SetProperty(ref coursenames, value);
        }

        public List<string> InstructNames
        {
            get => instructnames; set => SetProperty(ref instructnames, value);
        }

        public List<Models.Course> Courses
        {
            get => courses; set => SetProperty(ref courses, value);
        }

        public List<Models.Term> Terms
        {
            get => terms; set => SetProperty(ref terms, value);
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

        private List<string> GetInstructorNames(List<Models.Instructor> instructlist)
        {
            List<string> nameslist = new List<string>();
            foreach (var instruct in instructlist)
            {
                nameslist.Add(instruct.Name);
            }
            return nameslist;
        }

        private List<string> GetCourseNames(List<Models.Course> courselist)
        {
            List<string> nameslist = new List<string>();
            foreach (var course in courselist)
            {
                nameslist.Add(course.Name);
            }
            return nameslist;
        }

        private List<string> GetTermNames(List<Models.Term> termlist)
        {
            List<string> nameslist = new List<string>();
            foreach (var term in termlist)
            {
                nameslist.Add(term.Name);
            }
            return nameslist;
        }

        private int GetInstructorID(int instructorindex)
        {
            int id = Instructors[instructorindex].ID;
            return id;
        }

        private int GetTermID(int termindex)
        {
            int id = Terms[termindex].TermID;
            return id;
        }

        private int GetCourseID(int courseindex)
        {
            int id = Courses[courseindex].ID;
            return id;
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            Models.Student student = new Models.Student()
            {
                Name = Name,
                Email = Email,
                Phone = Phone,
                InstructorID = 0,//GetInstructorID(InstructorIndex),
                CurrentTermID = 0,//GetTermID(TermIndex),
                CurrentCourseID = 0//GetCourseID(CourseIndex)
            };

            await App.DataBase.SaveStudent(student);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
