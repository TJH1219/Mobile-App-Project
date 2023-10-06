using System;
using System.Collections.Generic;
using System.Text;
using static Xamarin.Essentials.Permissions;
using Xamarin.Essentials;
using Xamarin.Forms;
using MobileApp.Models;

namespace MobileApp.ViewModels
{
    public class AddAssessmentViewModel : BaseViewModel
    {
        private string name;
        private string description;
        private bool isperformance;
        private DateTime startdate;
        private DateTime enddate;
        private List<Models.Course> courselist;
        private List<string> coursenames;
        private List<string> statuslist;
        private int selectedindexname;
        private int selectedstatus;
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }


        public int SelectedStatus
        {
            get => selectedstatus; set => SetProperty(ref selectedstatus, value);
        }

        public List<string> StatusList
        {
            get => statuslist; set => SetProperty(ref statuslist, value);
        }

        public int SelectedNameIndex
        {
            get => selectedindexname; set => SetProperty(ref selectedindexname, value);
        }

        public List<string> CourseNames
        {
            get => coursenames; set => SetProperty(ref coursenames, value);
        }

        public List<Models.Course> CourseList
        {
            get => courselist; set => SetProperty(ref courselist, value);
        }

        public string Name
        {
            get => name; set => SetProperty(ref name, value);
        }

        public string Description
        {
            get => description; set => SetProperty(ref description, value);
        }

        public bool IsPerformance
        {
            get => isperformance; set => SetProperty(ref isperformance, value);
        }

        public DateTime EndDate
        {
            get => enddate;
            set => SetProperty(ref enddate, value);
        }

        public DateTime StartDate
        {
            get => startdate;
            set => SetProperty(ref startdate, value);
        }

        public AddAssessmentViewModel()
        {
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            SaveCommand = new Command(OnSave);
            CancelCommand = new Command(OnCancel);
            CourseList = App.DataBase.GetAllCourses().Result;
            CourseNames = GetCourseNames(CourseList);
            StatusList = MakeStatusList();
        }

        public List<string> GetCourseNames(List<Course> courselist)
        {
            List<string> names = new List<string>();
            foreach (Course course in courselist)
            {
                names.Add(course.Name);
            }
            return names;
        }

        public List<string> MakeStatusList()
        {
            List<string> list = new List<string>();
            for (int i = 0; i < 4; i++)
            {
                if (i == 0) list.Add("Plan to take");
                else if (i == 1) list.Add("In progress");
                else if (i == 2) list.Add("Complete");
                else if (i == 3) list.Add("Dropped");
            }
            return list;
        }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync($"..\\..\\..");
        }

        private async void OnSave()
        {
            Models.Assessment assess = new Models.Assessment()
            {
                Name = Name,
                Description = Description,
                StartDate = StartDate,
                EndDate = EndDate,
                CourseID = CourseList[SelectedNameIndex].ID,
                Status = SelectedStatus,
                IsPerformance = IsPerformance
            };

            await App.DataBase.SaveAssessment(assess);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync($"..\\..\\..");
        }
    }
}
