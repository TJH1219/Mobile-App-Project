using MobileApp.Models;
using MobileApp.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    internal class ModifyTermViewModel : BaseViewModel
    {
        private string termid;
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        private DateTime startdate;
        private DateTime enddate;
        private List<Models.Term> terms;
        private string name;
        private List<string> termnames;
        private int termindex;
        private Term termdata;

        public int TermIndex
        {
            get => termindex; set
            {
                Models.Term term = Terms[value];
                Name = term.Name;
                EndDate = term.EndDate;
                StartDate = term.StartDate;
                SetProperty(ref termindex, value);
            }
        }

        public Term TermData
        {
            get => termdata; set => SetProperty(ref termdata, value);
        }

        public string TermId
        {
            get => termid; set
            {
                TermId = value;
                LoadItemId(value);
            }
        }

        public List<string> TermNames
        {
            get => termnames; set => SetProperty(ref termnames, value);
        }

        public List<Models.Term> Terms
        {
            get => terms; set => SetProperty(ref terms, value);
        }

        public string Name
        {
            get => name; set => SetProperty(ref name, value);
        }

        public DateTime StartDate
        {
            get => startdate; set => SetProperty(ref startdate, value);
        }

        public DateTime EndDate
        {
            get => enddate; set => SetProperty(ref enddate, value);
        }

        public ModifyTermViewModel()
        {
            Terms = App.DataBase.GetAllTerms().Result;
            StartDate = DateTime.Today;
            EndDate = DateTime.Today;
            SaveCommand = new Command(OnSave);
            CancelCommand = new Command(OnCancel);
            TermNames = GetTermNames();
            TermIndex = 0;
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        public async void LoadItemId(string itemid)
        {
            try
            {
                List<Term> termlist = await App.DataBase.GetAllTerms();
                TermData = termlist[Convert.ToInt32(TermId)];
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to load term data");
            }
        }

        private List<string> GetTermNames()
        {
            List<string> names = new List<string>();
            foreach(var term in Terms)
            {
                names.Add(term.Name);
            }
            return names;
        }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync($"..\\..");
        }

        private int GetTermID(int termindex)
        {
            int id = Terms[TermIndex].TermID;
            return id;
        }

        private async void OnSave()
        {
            Models.Term term = new Models.Term()
            {
                TermID = GetTermID(TermIndex),
                Name = Name,
                StartDate = StartDate,
                EndDate = EndDate
            };

            await App.DataBase.UpdateTerm(term);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync($"..\\..");
        }
    }
}
