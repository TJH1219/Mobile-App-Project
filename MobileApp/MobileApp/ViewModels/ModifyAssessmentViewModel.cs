using MobileApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    [QueryProperty(nameof(AssessmentID), nameof(AssessmentID))]
    public class ModifyAssessmentViewModel : BaseViewModel
    {
        private int courseid;
        private bool isperformance;
        private int status;
        private List<string> statuslist;
        private string assessmentID;
        private string name;
        private string description;
        private DateTime startdate;
        private DateTime enddate;
        private List<Models.Assessment> assessmentlist;
        private List<Models.Course> courselist;
        private List<string> coursenames;
        private int selectedindexname;
        private int selectedstatus;
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        public ModifyAssessmentViewModel()
        {
            SaveCommand = new Command(OnSave);
            CancelCommand = new Command(OnCancel);
            CourseList = App.DataBase.GetAllCourses().Result;
            CourseNames = GetCourseNames(CourseList);
            StatusList = MakeStatusList();
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
        }

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

        public int CourseID
        {
            get => courseid; set => SetProperty(ref courseid, value);
        }

        public DateTime StartDate
        {
            get => startdate; set => SetProperty(ref startdate, value);
        }

        public DateTime EndDate
        {
            get => enddate; set => SetProperty(ref enddate, value);
        }

        public string AssessmentID
        {
            get => assessmentID; set
            {
                SetProperty(ref assessmentID, value);
                LoadItemId(value);
            }
        }

        public int Status
        {
            get => status; set => SetProperty(ref status, value);
        }

        public List<Models.Assessment> AssessmentList
        {
            get => assessmentlist; set => SetProperty(ref assessmentlist, value);
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

        public List<string> GetCourseNames(List<Course> courselist)
        {
            List<string> names = new List<string>();
            foreach(Course course in courselist)
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

        public async void LoadItemId(string itemId)
        {
            try
            {
                var items = await App.DataBase.GetAllAssessments();
                foreach (var Item in items)
                {
                    if (Item.ID == Convert.ToInt32(itemId))
                    {
                        Name = Item.Name;
                        Description = Item.Description;
                        IsPerformance = Item.IsPerformance;
                        Status = Item.Status;
                        CourseID = Item.CourseID;
                    }
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..\\..\\..\\..");
        }

        private async void OnSave()
        {
            Models.Assessment assess = new Models.Assessment()
            {
                ID = Convert.ToInt32(AssessmentID),
                Name = Name,
                Description = Description,
                StartDate = StartDate,
                EndDate = EndDate,
                CourseID = CourseList[SelectedNameIndex].ID,
                Status = SelectedStatus,
                IsPerformance = IsPerformance
            };

            await App.DataBase.UpdateAssessment(assess);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync($"..\\..\\..\\..");
        }
    }
}
