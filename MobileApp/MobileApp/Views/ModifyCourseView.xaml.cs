using MobileApp.Models;
using MobileApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.ViewModels
{
    public partial class ModifyCourseView : ContentPage
    {
        public ModifyCourseView()
        {
            InitializeComponent();
            BindingContext = new ModifyCourseViewModel();
        }

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {

        }
    }
}