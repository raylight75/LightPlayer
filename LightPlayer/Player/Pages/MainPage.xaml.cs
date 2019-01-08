using Xamarin.Forms;
using Player.ViewModels;
using Player.Interfaces;

namespace Player.Pages
{
    public partial class MainPage : TabbedPage
    {
        public bool isLoaded { get; set; }

        public MainPage()
        {
            InitializeComponent();            
            BindingContext = new MainVM();
        }      

        protected override bool OnBackButtonPressed()
        {
            if (Device.RuntimePlatform == Device.Android)
                DependencyService.Get<ICloseApplication>().CloseApp();
            return base.OnBackButtonPressed();
        }
    }
}
