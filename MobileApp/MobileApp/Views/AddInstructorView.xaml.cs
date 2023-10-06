using MobileApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views.AddViews
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddInstructorView : ContentPage
	{
		public AddInstructorView ()
		{
			InitializeComponent ();
			BindingContext = new NewInstructorViewModel();
		}
	}
}