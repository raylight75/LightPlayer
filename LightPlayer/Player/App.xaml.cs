using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Player.Pages;
using DLToolkit.Forms.Controls;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Player
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            FlowListView.Init();
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
