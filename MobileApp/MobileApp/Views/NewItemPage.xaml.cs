﻿using MobileApp.Models;
using MobileApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }
        public NewItemViewModel _viewmodel;

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = _viewmodel = new NewItemViewModel();
        }
    }
}