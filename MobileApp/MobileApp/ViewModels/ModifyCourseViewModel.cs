using MobileApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;


namespace MobileApp.ViewModels
{
    [QueryProperty(nameof(ItemID), nameof(ItemID))]
    internal class ModifyCourseViewModel : BaseViewModel
    {
        private string itemId;
        public string Id { get; set; }
        private string text;
        private string description;
        private DateTime currentdate;
        private string cr_Date;
        private DateTime startdate;
        private DateTime enddate;
        private int credits;
        private int assess1index;
        private int assess2index;
        private List<Instructor> instructors = App.DataBase.GetAllInstructors().Result;
        private List<Assessment> assesslist = App.DataBase.GetAllAssessments().Result;
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        private List<string> instructornamelist;
        private List<Term> termlist = App.DataBase.GetAllTerms().Result;
        private List<string> termnames;
        private int termindex;
        private int instructindex;
        private int status;
        private List<string> statuslist;
        private int selectedstatus;
        private List<string> assessmentnames;
        private int courseID;

        public string ItemID
        {
            get => itemId; set 
            {
                SetProperty(ref itemId, value);
                LoadItemId(value);
            }
        }

        public List<string> AssessmentNames
        {
            get => assessmentnames; set => SetProperty(ref assessmentnames, value);
        }

        public int Status
        {
            get => status; set => SetProperty(ref status, value);
        }

        public int SelectedStatus
        {
            get => selectedstatus; set => SetProperty(ref selectedstatus, value);
        }

        public List<string> StatusList
        {
            get => statuslist; set => SetProperty(ref statuslist, value);
        }

        public int InstructIndex
        {
            get => instructindex; set => SetProperty(ref instructindex, value);
        }

        public int TermIndex
        {
            get => termindex; set => SetProperty(ref termindex, value);
        }

        public ModifyCourseViewModel()
        {
            AssessmentNames = GetAssessmentNames(AssessList);
            currentdate = DateTime.Now;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            InstructorNameList = GetInstructorList(instructors);
            StatusList = MakeStatusList();
            TermNames = GetTermNameList(termlist);
            CR_Date = currentdate.ToShortDateString();
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
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

        //Get The names of the instructors
        private List<string> GetInstructorList(List<Instructor> list)
        {
            List<string> courselist = new List<string>();
            foreach (Instructor Course in list)
            {
                courselist.Add(Course.Name);
            }
            return courselist;
        }

        private List<string> GetTermNameList(List<Term> list)
        {
            List<string> courselist = new List<string>();
            foreach (Term Course in list)
            {
                courselist.Add(Course.Name);
            }
            return courselist;
        }

        private List<string> GetAssessmentNames(List<Assessment> list)
        {
            List<string> assesslist = new List<string>();
            foreach(Assessment Assessment in list)
            {
                assesslist.Add(Assessment.Name);
            }
            return assesslist;
        }

        public List<string> TermNames
        {
            get => termnames; set => SetProperty(ref termnames, value);
        }

        public List<string> InstructorNameList { get => instructornamelist; set => SetProperty(ref instructornamelist, value); }

        public List<Term> TermList
        {
            get => termlist; set => SetProperty(ref termlist, value);
        }

        public string CR_Date
        {
            get => cr_Date;
            set => SetProperty(ref cr_Date, value);
        }

        private bool ValidateSave()
        {
            return !string.IsNullOrWhiteSpace(text)
                && !string.IsNullOrWhiteSpace(description);
        }

        public DateTime CurrentDate
        {
            get => currentdate;
            set => SetProperty(ref currentdate, value);
        }

        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public DateTime StartDate
        {
            get => startdate;
            set => SetProperty(ref startdate, value);
        }

        public DateTime EndDate
        {
            get => enddate;
            set => SetProperty(ref enddate, value);
        }

        public int Credits
        {
            get => credits; set => SetProperty(ref credits, value);
        }

        public int Assess1Index
        {
            get => assess1index; set => SetProperty(ref assess1index, value);
        }

        public int Assess2Index
        {
            get => assess2index; set => SetProperty(ref assess2index, value);
        }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync($"..\\..\\..");
        }

        private Instructor GetSelectedInstructor()
        {
            Instructor instruct = new Instructor();
            instruct = instructors[InstructIndex];
            return instruct;
        }

        private Term GetSelectedTerm()
        {
            Term term = new Term();
            term = TermList[TermIndex];
            return term;
        }

        private async void OnSave()
        {
            Course newCourse = new Course()
            {
                ID = Convert.ToInt32(itemId),
                Name = Text,
                TermID = GetSelectedTerm().TermID,
                Description = Description,
                EndDate = EndDate,
                StartDate = StartDate,
                InstructorID = GetSelectedInstructor().ID,
                Credits = Credits,
                Status = SelectedStatus,
            };

            await App.DataBase.UpdateCourse(newCourse);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync($"..\\..\\..");
        }

        public async void LoadItemId(string itemId)
        {
            try
            {
                var items = await App.DataBase.GetAllCourses();
                foreach (var Item in items)
                {
                    if (Item.ID == Convert.ToInt32(itemId))
                    {
                        Text = Item.Name;
                        Description = Item.Description;
                        EndDate = Item.EndDate;
                        StartDate = Item.StartDate;
                        Credits = Item.Credits;
                    }
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }

        public List<Assessment> AssessList
        {
            get => assesslist; set => SetProperty(ref assesslist, value);
        }

        public List<Instructor> Instructors
        {
            get => instructors;
            set => SetProperty(ref instructors, value);
        }
    }
}
