using MobileApp.ViewModels;
using MobileApp.Views;
using MobileApp.Views.AddViews;
using MobileApp.Views.ModifyViews;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MobileApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(AddTermPage), typeof(AddTermPage));
            Routing.RegisterRoute(nameof(ModifyCourseView), typeof(ModifyCourseView));
            Routing.RegisterRoute(nameof(AddInstructorView), typeof(AddInstructorView));
            Routing.RegisterRoute(nameof(AddUser), typeof(AddUser));
            Routing.RegisterRoute(nameof(ModifyUser), typeof(ModifyUser));
            Routing.RegisterRoute(nameof(AddNewStudent), typeof(AddNewStudent));
            Routing.RegisterRoute(nameof(ModifyStudent), typeof(ModifyStudent));
            Routing.RegisterRoute(nameof(ModifyTerm), typeof(ModifyTerm));
            Routing.RegisterRoute(nameof(Views.EditInstructorView), typeof(Views.EditInstructorView));
            Routing.RegisterRoute(nameof(Views.AddAssessmentPage), typeof(Views.AddAssessmentPage));
            Routing.RegisterRoute(nameof(Views.ModifyAssessment), typeof(Views.ModifyAssessment));
            Routing.RegisterRoute(nameof(Views.TermDetailView), typeof(Views.TermDetailView));
            Routing.RegisterRoute(nameof(Views.AssessmentDetailView), typeof(Views.AssessmentDetailView));
            Routing.RegisterRoute(nameof(Views.ShareNotesPage),typeof(Views.ShareNotesPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
