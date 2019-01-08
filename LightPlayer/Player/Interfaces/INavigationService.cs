using Player.Pages;
using Xamarin.Forms;

namespace Player.Interfaces
{
    public interface INavigationService
    {
        void NavigateToTabb(int tabb);
        MainPage GetCurrentPage();
    }
}
