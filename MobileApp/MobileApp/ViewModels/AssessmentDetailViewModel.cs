using MobileApp.Models;
using MobileApp.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Xamarin.Forms;
using static Xamarin.Essentials.Permissions;

namespace MobileApp.ViewModels
{
    [QueryProperty(nameof(AssessmentID), nameof(AssessmentID))]
    internal class AssessmentDetailViewModel : BaseViewModel
    {
        #region Field
        public Command GotoModifyAssessment { get; }
        public Command DeleteCommand { get; }
        public Command OpenShareNotes { get; }
        private Models.Assessment assessment;
        private string assessmentID;
        private string assessmentName;
        public Command SetReminderCommand { get; set; }
        private string description;
        private bool isperformance;
        private string reminderpick;
        private string typetext;
        private List<string> reminderlist;
        private DateTime startdate;
        private DateTime enddate;
        private int status;
        private string setnotificationconfirm;
        #endregion 

        public AssessmentDetailViewModel()
        {
            OpenShareNotes = new Command(ShareNotes);
            DeleteCommand = new Command(DeleteAssessment);
            GotoModifyAssessment = new Command(ModifyAssessmentGoto);
            ReminderOptionList = BuildNotificationlist();
            TypeText = SetTypeText(IsPerformance);
            SetReminderCommand = new Command(SetReminder);
        }

        #region Properties
        public string SetNotificationConfirm
        {
            get => setnotificationconfirm; set => SetProperty(ref setnotificationconfirm, value);
        }

        public string ReminderPick
        {
            get => reminderpick; set => SetProperty(ref reminderpick, value);
        }

        public Models.Assessment _Assessment
        {
            get => assessment; set => SetProperty(ref assessment, value);
        }

        public List<string> ReminderOptionList
        {
            get => reminderlist; set => SetProperty(ref reminderlist, value);
        }

        public string TypeText
        {
            get => typetext; set => SetProperty(ref typetext, value);
        }

        public int Status
        {
            get => status; set => SetProperty(ref status, value);
        }

        public bool IsPerformance
        {
            get => isperformance; set => SetProperty(ref isperformance, value);
        }

        public string AssessmentName
        {
            get => assessmentName; set => SetProperty(ref assessmentName, value);
        }

        public string Description
        {
            get => description; set => SetProperty(ref description, value);
        }

        public string AssessmentID
        {
            get => assessmentID; set
            {
                SetProperty(ref assessmentID, value);
                LoadItemId(Convert.ToString(value));
            }
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

        #endregion

        private string SetTypeText(bool isperformance)
        {
            string text;
            if (isperformance)
            {
                text = "Performance Assessment";
            }
            else
            {
                text = "Objective Assessment";
            }
            return text;
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

        private DateTime GetReminderTriggerDate(string pick)
        {
            DateTime _triggerdate = new DateTime();
            if (pick == "Start Date")
            {
                _triggerdate = _Assessment.StartDate;
            }
            else
            {
                _triggerdate = _Assessment.EndDate;
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
                    ReferenceID = _Assessment.ID,
                    Name = _Assessment.Name + " Reminder",
                    TriggerDate = _triggerdate,
                    Desc = "Reminder for " + _Assessment.Name,
                };
                await App.DataBase.SaveReminder(reminder);
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to make reminder");
            }
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
                        _Assessment = Item;
                        AssessmentName = Item.Name;
                        Description = Item.Description;
                        IsPerformance = Item.IsPerformance;
                        Status = Item.Status;
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

        public async void ModifyAssessmentGoto()
        {
            await Shell.Current.GoToAsync($"{nameof(ModifyAssessment)}?{nameof(ModifyAssessmentViewModel.AssessmentID)}={AssessmentID}");
        }

        public async void DeleteAssessment()
        {
            await App.DataBase.DeleteAssessment(_Assessment);
            await Shell.Current.GoToAsync("..\\..\\..");
        }

        public async void ShareNotes()
        {
            await Shell.Current.GoToAsync($"{nameof(ShareNotesPage)}");
        }
    }
}
