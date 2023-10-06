using MobileApp.Models;
using MobileApp.Views;
using MobileApp.ViewModels;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections.Generic;
using MobileApp.Views.AddViews;
using System.Collections.ObjectModel;

namespace MobileApp.ViewModels
{
    // This is the Course Detail ViewModel
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        #region Fields
        private string term;
        private string setnotificationconfirm;
        public Command AddInstructorCom { get; set; }
        public Command ModifyCourse { get; set; }
        public Command DeleteCommand { get; set; }
        public Command EditInstructorCommand { get; set; }
        public Command GotoAddAssessment { get; set; }
        public Command GotoModifyAssessment { get; }
        public Command SetReminderCommand { get; set; }
        public Command LoadItemsCommand { get; }
        public Command<Models.Assessment> ItemTapped { get; }
        private Models.Course course;
        private Models.Instructor instruct;
        //options for the reminder picker
        private List<string> reminderlist;
        private string assess1_name;
        private string assess2_name;
        private string instruct_Name;
        private string instruct_email;
        private int credits;
        private string itemId;
        private string text;
        private string description;
        private string reminderpick;
        private DateTime startdate;
        private DateTime enddate;
        private DateTime reminderTriggerDate;
        public string Id { get; set; }
        private Models.Assessment _selectedItem;
        private ObservableCollection<Models.Assessment> assessments;
        public Command OpenShareNotes { get; }
        private string status;
        private List<string> statuslist;
        #endregion

        public ItemDetailViewModel()
        {
            ReminderOptionList = BuildNotificationlist();
            StatusList = MakeStatusList();
            OpenShareNotes = new Command(ShareNotes);
            SetReminderCommand = new Command(SetReminder);
            ItemTapped = new Command<Assessment>(OnItemSelected);
            AddInstructorCom = new Command(AddInstructor);
            ModifyCourse = new Command(ChangeDate);
            DeleteCommand = new Command(DeleteItem);
            EditInstructorCommand = new Command(GotoEditInstructor);
            GotoAddAssessment = new Command(AddAssessment);
            GotoModifyAssessment = new Command(ModifyAssessmentGoto);
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            AssessmentsList = new ObservableCollection<Assessment>();
        }

        #region Properties
        public string SetNotificationConfirm
        {
            get => setnotificationconfirm; set => SetProperty(ref setnotificationconfirm, value);
        }

        public List<string> StatusList
        {
            get => statuslist; set => SetProperty(ref statuslist, value);
        }

        public ObservableCollection<Models.Assessment> AssessmentsList
        {
            get => assessments; set => SetProperty(ref assessments, value);
        }

        public string Status
        {
            get => status; set => SetProperty(ref status, value);
        }

        public Models.Assessment SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        public List<string> ReminderOptionList
        {
            get => reminderlist; set => SetProperty(ref reminderlist, value);
        }

        public DateTime ReminderTriggerDate
        {
            get => reminderTriggerDate; set => SetProperty(ref reminderTriggerDate, value);
        }

        public string ReminderPick
        {
            get => reminderpick; set => SetProperty(ref reminderpick, value);
        }

        public string Term
        {
            get => term; set => SetProperty(ref term, value);
        }

        public Models.Course _Course
        {
            get => course; set => SetProperty(ref course, value);
        }

        public int Credits
        {
            get => credits; set => SetProperty(ref credits, value);
        }

        public string Assess1_Name
        {
            get => assess1_name;
            set => SetProperty(ref assess1_name, value);
        }

        public string Assess2_Name
        {
            get => assess1_name;
            set => SetProperty(ref assess2_name, value);
        }


        public string Instruct_Name
        {
            get => instruct_Name;
            set => SetProperty(ref instruct_Name, value);
        }

        public string Instruct_Email
        {
            get => instruct_email;
            set => SetProperty(ref instruct_email, value);
        }

        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public Models.Instructor Instruct
        {
            get => instruct;
            set => SetProperty(ref instruct, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
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

        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadItemId(value);
            }
        }
        #endregion

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

        private string GetInstructorName(int id)
        {
            var items = App.DataBase.GetAllInstructors().Result;
            string name = "";
            foreach (var Item in items)
            {
                if (Item.ID == id)
                {
                    name = Item.Name;
                }
            }
            return name;
        }

        private string GetTermName(Course course)
        {
            List<Term> termlist = App.DataBase.GetAllTerms().Result;
            string name = "";
            foreach(Term term in termlist)
            {
                if(term.TermID == course.TermID)
                {
                    name = term.Name;
                }
            }
            return name;
        }

        private List<string> BuildNotificationlist()
        {
            List<string> notificationlist = new List<string>();
            for (int i = 0; i < 2; i++)
            {
                if (i == 0)
                {
                    notificationlist.Add("Start Date");
                }
                else
                {
                    notificationlist.Add("End Date");
                }
            }
            return notificationlist;
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        private DateTime GetReminderTriggerDate(string pick)
        {
            DateTime _triggerdate = new DateTime();
            if (pick == "Start Date")
            {
                _triggerdate = _Course.StartDate;
            }
            else
            {
                _triggerdate = _Course.EndDate;
            }
            return _triggerdate;
        }

        private async void SetReminder()
        {
            DateTime _triggerdate = GetReminderTriggerDate(ReminderPick);
            SetNotificationConfirm = "Notification Set";
            try
            {
                Models.Reminders reminder = new Models.Reminders()
                {
                    Type = 0,
                    ReferenceID = _Course.ID,
                    Name = _Course.Name + " Reminder",
                    TriggerDate = _triggerdate,
                    Desc = "Reminder for " + _Course.Name,
                };
                await App.DataBase.SaveReminder(reminder);
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to make reminder");
            }
        }

        private string GetInstructEmail(int id)
        {
            var items = App.DataBase.GetAllInstructors().Result;
            string email = "";
            foreach (var Item in items)
            {
                if (Item.ID == id)
                {
                    email = Item.Email;
                }
            }
            return email;
        }
        public async void LoadItemId(string itemId)
        {
            try
            {
                var items = await App.DataBase.GetAllCourses();
                foreach(var Item in items)
                {
                    if(Item.ID == Convert.ToInt32(itemId))
                    {
                        _Course = Item;
                        Text = Item.Name;
                        Description = Item.Description;
                        StartDate = Item.StartDate;
                        Instruct_Name = GetInstructorName(Item.InstructorID);
                        Instruct_Email = GetInstructEmail(Item.InstructorID);
                        EndDate= Item.EndDate;
                        Credits = Item.Credits;
                        Term = GetTermName(_Course);
                        Status = StatusList[Item.Status];
                    }
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }

        async void OnItemSelected(Models.Assessment assess)
        {
            if (assess == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(AssessmentDetailView)}?{nameof(AssessmentDetailViewModel.AssessmentID)}={Convert.ToInt32(assess.ID)}");
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;
            try
            {
                this.AssessmentsList.Clear();
                var CourseList = await App.DataBase.GetAllAssessments();
                foreach (Assessment assess in CourseList)
                {
                    if (assess.CourseID == Convert.ToInt32(ItemId)) 
                    {
                        this.AssessmentsList.Add(assess);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async void ChangeDate()
        {
            await Shell.Current.GoToAsync($"{nameof(ModifyCourseView)}?{nameof(ModifyCourseViewModel.ItemID)}={Convert.ToString(_Course.ID)}");
        }

        public async void AddAssessment()
        {
            await Shell.Current.GoToAsync($"{nameof(AddAssessmentPage)}");
        }

        public async void ModifyAssessmentGoto()
        {
            await Shell.Current.GoToAsync($"{nameof(ModifyAssessment)}");
        }

        public async void GotoEditInstructor()
        {
            await Shell.Current.GoToAsync($"{nameof(EditInstructorView)}");
        }

        public async void AddInstructor()
        {
            await Shell.Current.GoToAsync($"{nameof(AddInstructorView)}");
        }

        public async void DeleteItem(object obj)
        {
            await App.DataBase.DeleteCourse(_Course);
            await Shell.Current.GoToAsync($"..\\..");
        }

        public async void ShareNotes()
        {
            await Shell.Current.GoToAsync($"{nameof(ShareNotesPage)}");
        }
    }
}
