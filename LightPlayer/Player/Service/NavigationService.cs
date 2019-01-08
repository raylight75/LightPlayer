using Player.Interfaces;
using Player.Pages;
using Player.Service;
using System.Linq;
using Xamarin.Forms;

[assembly: Dependency(typeof(NavigationService))]
namespace Player.Service
{
    class NavigationService : INavigationService
    {
        public void NavigateToTabb(int tabb)
        {
            var currentPage = GetCurrentPage();
            currentPage.CurrentPage = currentPage.Children[tabb];
        }

        public MainPage GetCurrentPage()
        {
            var currentPage = Application.Current.MainPage.Navigation.NavigationStack.LastOrDefault() as MainPage;
            return currentPage;
        }       
    }
}
