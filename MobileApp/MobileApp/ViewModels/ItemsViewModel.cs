using MobileApp.Models;
using MobileApp.Views;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    //This is the terms viewmodel
    public class ItemsViewModel : BaseViewModel
    {
        private List<string> termnamelist;
        private Models.Term _selectedItem;
        private List<Models.Term> termlist;
        private int _selectedTerm;
        private List<Models.Course> courselist;
        public ObservableCollection<Models.Term> Terms{ get; set; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<Models.Term> ItemTapped { get; }
        public Command AddTermCommand { get; }
        public Command ModifyTermCommand { get; }
        public Command DeleteTermCommand { get; }
        public Command SetReminderCommand { get; set; }
        private List<string> reminderlist;
        private string reminderpick;

        public ItemsViewModel()
        {
            ReminderOptionList = BuildNotificationlist();
            TermList = App.DataBase.GetAllTerms().Result;
            CourseList = App.DataBase.GetAllCourses().Result;
            Title = "Terms";
            Terms = new ObservableCollection<Models.Term>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            ItemTapped = new Command<Term>(OnItemSelected);
            AddItemCommand = new Command(OnAddItem);
            AddTermCommand = new Command(OnAddTerm);
            ModifyTermCommand = new Command(OnModifyTerm);
            TermNameList = GetTermNames(TermList);
        }

        public string ReminderPick
        {
            get => reminderpick; set => SetProperty(ref reminderpick, value);
        }

        public List<string> ReminderOptionList
        {
            get => reminderlist; set => SetProperty(ref reminderlist, value);
        }

        private List<string> GetTermNames(List<Term> list)
        {
            List<string> termlist = new List<string>();
            foreach (Term term in list)
            {
                termlist.Add(term.Name);
            }
            return termlist;
        }

        public List<string> TermNameList
        {
            get => termnamelist; set => SetProperty(ref termnamelist, value);
        }

        public List<Models.Course> CourseList
        {
            get => courselist; set => SetProperty(ref courselist, value);
        }

        public int SelectedTerm
        {
            get => _selectedTerm; 
            set 
            {
                if (_selectedTerm != value)
                {
                    SetProperty(ref _selectedTerm, value);
                }
            }
        }

        public List<Models.Term> TermList
        {
            get => termlist; set => SetProperty(ref termlist, value);
        }

        async Task AddButtonClick()
        {
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?");
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

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;
            try
            {
                this.Terms.Clear();
                var CourseList = await App.DataBase.GetAllTerms();
                foreach (Term term in CourseList)
                {
                    this.Terms.Add(term);
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

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public Models.Term SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(AddTermPage));
        }

        private async void OnAddTerm(object obj)
        {
            await Shell.Current.GoToAsync(nameof(AddTermPage));
        }

        private async void DeleteTerm(int termindex)
        {
            await App.DataBase.DeleteTerm(TermList[termindex]);
            await Shell.Current.GoToAsync("..");
        }

        private async void OnModifyTerm(object obj)
        {
            await Shell.Current.GoToAsync(nameof(ModifyTerm));
        }

        async void OnItemSelected(Models.Term term)
        {
            if (term == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(TermDetailView)}?{nameof(TermDetailsViewModel.TermId)}={Convert.ToString(term.TermID)}");
        }
    }
}