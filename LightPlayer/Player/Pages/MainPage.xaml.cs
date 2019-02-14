using Xamarin.Forms;
using Player.ViewModels;
using Player.Interfaces;
using System;

namespace Player.Pages
{
    public partial class MainPage : TabbedPage
    {        
        public bool isLoaded { get; set; }

        public MainPage()
        {
            InitializeComponent();
            DependencyService.Get<INavigationService>().SetNav(Navigation);
            BindingContext = new MainVM();
        }      

        protected override bool OnBackButtonPressed()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (await DisplayAlert("Would you like to close player", "You will need to load playlist again", "Yes", "No"))
                    {
                        base.OnBackButtonPressed();
                        DependencyService.Get<ICloseApplication>().CloseApp();
                    }
                });                
            }
            return true;
        }        
    }
}
