using Player.Interfaces;
using Player.Service;
using Xamarin.Forms;

[assembly: Dependency(typeof(NavigationService))]
namespace Player.Service
{
    class NavigationService : INavigationService
    {
        public INavigation Navigation { get; set; }        

        public void SetNav(INavigation _navigation)
        {
            Navigation = _navigation;
        }                       
    }
}
