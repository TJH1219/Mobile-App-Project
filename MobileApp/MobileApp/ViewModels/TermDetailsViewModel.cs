using MobileApp.Models;
using MobileApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    [QueryProperty(nameof(TermId), nameof(TermId))]
    public class TermDetailsViewModel : BaseViewModel
    {
        public Command AddCourse { get; set; }
        private string termId;
        private string name;
        public Command<Models.Course> ItemTapped { get; }
        private DateTime startdate;
        private DateTime enddate;
        private ObservableCollection<Models.Course> courses;
        private Models.Course _selectedItem;
        public Command SetReminderCommand { get; set; }
        public Command LoadItemsCommand { get; }
        public Command DeleteTermCommand { get; }
        public Command ModifyTermCommand { get; }
        private List<Term> termlist;
        private List<string> reminderoptionslist;
        private string reminderchoice;

        public string ReminderChoice
        {
            get => reminderchoice; set => SetProperty(ref reminderchoice, value);
        }

        public List<string> ReminderOptionList
        {
            get => reminderoptionslist; set => SetProperty(ref reminderoptionslist, value);
        }

        public List<Term> Termlist
        {
            get => termlist; set => SetProperty(ref termlist, value);
        }

        public ObservableCollection<Models.Course> Courses
        {
            get => courses; set => SetProperty(ref courses, value);
        }

        public DateTime StartDate
        {
            get => startdate; set => SetProperty(ref startdate, value);
        }

        public DateTime EndDate
        {
            get => enddate; set => SetProperty(ref enddate, value);
        }

        public string Name
        {
            get => name; set => SetProperty(ref name, value);
        }

        public string TermId
        {
            get
            {
                return termId;
            }
            set
            {
                SetProperty(ref termId, value);
                LoadItemId(value);
            }
        }

        public Models.Course SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        public TermDetailsViewModel()
        {
            ReminderOptionList = BuildNotificationlist();
            ModifyTermCommand = new Command(OnModifyTerm);
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            AddCourse = new Command(OnCourseAdd);
            Termlist = App.DataBase.GetAllTerms().Result;
            DeleteTermCommand = new Command(OnDeleteTerm);
            ItemTapped = new Command<Course>(OnItemSelected);
            SetReminderCommand = new Command(SetReminder);
            Courses = new ObservableCollection<Course>();
        }


        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;
            try
            {
                this.Courses.Clear();
                var CourseList = await App.DataBase.GetAllCourses();
                foreach (Course course in CourseList)
                {
                    if (course.TermID == Convert.ToInt32(TermId))
                    {
                        this.Courses.Add(course);
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

        private DateTime GetReminderTriggerDate(string pick, Models.Term term)
        {
            DateTime _triggerdate = new DateTime();
            if (pick == "Start Date")
            {
                _triggerdate = term.StartDate;
            }
            else
            {
                _triggerdate = term.EndDate;
            }
            return _triggerdate;
        }

        private async void SetReminder()
        {
            Models.Term term = Termlist[Convert.ToInt32(TermId) - 1];
            DateTime _triggerdate = GetReminderTriggerDate(ReminderChoice, term);
            try
            {
                Models.Reminders reminder = new Models.Reminders()
                {
                    Type = 1,
                    ReferenceID = term.TermID,
                    Name = term.Name + " Reminder",
                    TriggerDate = _triggerdate,
                    Desc = "Reminder for " + term.Name,
                };
                await App.DataBase.SaveReminder(reminder);
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to make reminder");
            }
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

        private async void OnDeleteTerm()
        {
            Term term = new Term();
            foreach (var cr_term in Termlist)
            {
                if (cr_term.TermID == Convert.ToInt32(TermId))
                {
                        term = cr_term;
                }
            }
            await App.DataBase.DeleteTerm(term);
            await Shell.Current.GoToAsync("..");
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        private async void OnCourseAdd()
        {
            await Shell.Current.GoToAsync($"{nameof(NewItemPage)}");
        }

        public async void LoadItemId(string itemId)
        {
            try
            {
                var items = await App.DataBase.GetAllTerms();
                foreach (var Item in items)
                {
                    if (Item.TermID == Convert.ToInt32(itemId))
                    {
                        Name = Item.Name;
                        StartDate = Item.StartDate;
                        EndDate = Item.EndDate;
                    }
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }

        async void OnItemSelected(Models.Course course)
        {
            if (course == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={Convert.ToString(course.ID)}");
        }

        async void OnModifyTerm ()
        {
            await Shell.Current.GoToAsync($"{nameof(ModifyTerm)}");
        }
    }
}
