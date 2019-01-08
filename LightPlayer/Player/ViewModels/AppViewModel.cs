using Player.Commands;
using Player.Helpers;
using Player.Interfaces;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Player.ViewModels
{
    class AppViewModel
    {
        private INavigationService _navigationService;
        public ICommand ExitAppCommand { get; set; }
        public ICommand SelectedTabCommand { get; set; }

        public AppViewModel()
        {
            NavigationService(DependencyService.Get<INavigationService>());
            LoadCommands();           
        }
        
        private void NavigationService(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        private void LoadCommands()
        {            
            ExitAppCommand = new RelayCommand(async parameter => { await ExitApp(); }, Permision.CanExecute);
            SelectedTabCommand = new RelayCommand(SelectedTab, Permision.CanExecute);            
        }

        private void SelectedTab(object p)
        {
            _navigationService.NavigateToTabb(1);
        }

        private async Task ExitApp()
        {
            if (await Application.Current.MainPage.DisplayAlert("Would you like to close player", "You will need to load playlist again", "Yes", "No"))
            {
                var closer = DependencyService.Get<ICloseApplication>();
                closer?.CloseApp();
            }            
        }
    }
}
