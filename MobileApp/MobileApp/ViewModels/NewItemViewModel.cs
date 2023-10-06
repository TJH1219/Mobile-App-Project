using MobileApp.Models;
using MobileApp.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    public class NewItemViewModel : BaseViewModel
    {
        private List<string> instructornamelist;
        private List<string> termnamelist;
        private int selectedtermindex;
        private string text;
        private string description;
        private DateTime startdate;
        private DateTime enddate;
        private int instruct;
        private List<Models.Term> termlist;
        private int credits;
        private int assess1;
        private int assess2;
        private List<Models.Instructor> instructors;
        private List<Models.Assessment> assesslist;
        private List<string> assessnames;
        private Term termdata;
        private int status;
        private List<string> statuslist;
        private int selectedstatus;

        public Term TermData
        {
            get => termdata; set => SetProperty(ref termdata, value);
        }


        public List<string> AssessNames
        {
            get => assessnames; set => SetProperty(ref assessnames, value); 
        }

        public List<string> InstructorNameList
        {
            get => instructornamelist; set => SetProperty(ref instructornamelist, value);
        }


        public List<string> TermNameList
        {
            get => termnamelist; set => SetProperty(ref termnamelist, value);
        }

        public int SelectedTermIndex
        {
            get => selectedtermindex; set => SetProperty(ref selectedtermindex, value);
        } 

        public List<Models.Term> TermList
        {
            get => termlist; set => SetProperty(ref termlist, value);
        }

        public List<Models.Assessment> AssessList
        {
            get => assesslist; set => SetProperty(ref assesslist, value);
        }

        public List<Models.Instructor> Instructors
        {
            get => instructors;
            set => SetProperty(ref instructors, value);
        }

        //Constructor
        public NewItemViewModel()
        {
            StatusList = MakeStatusList();
            Instructors = App.DataBase.GetAllInstructors().Result;
            AssessList = App.DataBase.GetAllAssessments().Result;
            TermList = App.DataBase.GetAllTerms().Result;
            TermNameList = GetTermNames(TermList);
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
            instructors = App.DataBase.GetAllInstructors().Result;
            InstructorNameList = GetInstructNames(instructors);
            assesslist = App.DataBase.GetAllAssessments().Result;
            Startdate = DateTime.Now;
            Enddate = DateTime.Now;
            AssessNames = GetAssessNames(AssessList);
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

        public async void LoadItemId(string itemid)
        {
            try
            {
                List<Term> termlist = await App.DataBase.GetAllTerms();
                TermData = termlist[Convert.ToInt32(itemid)];
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to load term data");
            }
        }

        private List<string> GetInstructNames(List<Models.Instructor> list)
        {
            List<string> instructnames = new List<string>();
            foreach (Instructor instruct in list)
            {
                instructnames.Add(instruct.Name);
            }
            return instructnames;
        }

        private List<string> GetTermNames(List<Models.Term> list)
        {
            List<string> termnames = new List<string>();
            foreach(Term term in list)
            {
                termnames.Add(term.Name);
            }
            return termnames;
        }

        private List<string> GetAssessNames(List<Models.Assessment> list)
        {
            List<string> assessnames = new List<string>();
            foreach (Assessment assessment in list)
            {
                assessnames.Add(assessment.Name);
            }
            return assessnames;
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(text)
                && !String.IsNullOrWhiteSpace(description);
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

        public DateTime Startdate
        {
            get => startdate;
            set=> SetProperty(ref startdate, value);
        }

        public DateTime Enddate
        {
            get => enddate;
            set=> SetProperty(ref enddate, value);
        }

        public int Instruct
        {
            get => instruct; set => SetProperty(ref instruct, value);
        }

        public int Credits
        {
            get => credits; set => SetProperty(ref credits, value);
        }

        public int Assess1
        {
            get => assess1; set => SetProperty(ref assess1, value); 
        }

        public int Assess2
        {
            get => assess2; set => SetProperty(ref assess2, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync($"..\\..");
        }

        private async void OnSave()
        {
            Models.Course newCourse = new Models.Course()
            {
                Name = Text,
                Description = Description,
                TermID = TermList[SelectedTermIndex].TermID,
                EndDate = Enddate,
                StartDate = Startdate,
                InstructorID = instructors[Instruct].ID,
                Credits = Credits,
                Status = Status,
            };

            await App.DataBase.SaveCourse(newCourse);


            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync($"..\\..");
        }
    }
}
