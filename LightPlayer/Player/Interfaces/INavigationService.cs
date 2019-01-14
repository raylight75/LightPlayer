using Player.Pages;
using Xamarin.Forms;

namespace Player.Interfaces
{
    public interface INavigationService
    {
        INavigation Navigation { get; set; }
        void SetNav(INavigation _nav);                
    }
}
