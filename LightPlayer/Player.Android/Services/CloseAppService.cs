using Android.App;
using Xamarin.Forms;
using Player.Interfaces;
using Player.Droid.Services;
using System.Threading.Tasks;

[assembly: Dependency(typeof(CloseAppService))]
namespace Player.Droid.Services
{
    class CloseAppService : ICloseApplication
    {
        public void CloseApp()
        {
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());                       
        }
    }
}