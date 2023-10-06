using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    internal class ModifyStudentViewModel : BaseViewModel
    {
        private List<string> studentnames;
        private List<Models.Student> students;
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
        private int studentindex;
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        public Command DeleteCommand { get; }

        public ModifyStudentViewModel()
        {
            DeleteCommand = new Command(onDelete);
            Students = App.DataBase.GetAllStudents().Result;
            Instructors = App.DataBase.GetAllInstructors().Result;
            Terms = App.DataBase.GetAllTerms().Result;
            Courses = App.DataBase.GetAllCourses().Result;
            TermNames = GetTermNames(Terms);
            InstructNames = GetInstructorNames(Instructors);
            CourseNames = GetCourseNames(Courses);
            StudentNames = GetStudentNames(Students);
            SaveCommand = new Command(OnSave);
            CancelCommand = new Command(OnCancel);
            PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        public int StudentIndex
        {
            get => studentindex; 
            set
            { 
                if(studentindex != value) 
                {
                    ChangeSelectedStudent(value);
                    SetProperty(ref studentindex, value);
                }           
            }
                
        }

        public List<string> StudentNames
        {
            get => studentnames; set => SetProperty(ref studentnames, value);
        }

        public List<Models.Student> Students
        {
            get => students; set => SetProperty(ref students, value);
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

        private List<string> GetStudentNames(List<Models.Student> studentlist)
        {
            List<string> names = new List<string>();
            foreach (var student in studentlist)
            {
                names.Add(student.Name);
            }
            return names;
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

        private void ChangeSelectedStudent(int index)
        {
            Models.Student student = Students[index];
            Name = student.Name;
            Email= student.Email;
            Phone= student.Phone;
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

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private int GetStudentID(int studentindex)
        {
            int id = Students[studentindex].ID;
            return id;
        }

        private async void OnSave()
        {
            Models.Student student = new Models.Student()
            {
                ID = GetStudentID(studentindex),
                Name = Name,
                Email = Email,
                Phone = Phone,
                InstructorID = GetInstructorID(InstructorIndex),
                CurrentTermID = GetTermID(TermIndex),
                CurrentCourseID = GetCourseID(CourseIndex)
            };

            await App.DataBase.UpdateStudent(student);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void onDelete()
        {
            Models.Student student = Students[StudentIndex];
            await App.DataBase.DeleteStudent(student);
        }
    }
}
