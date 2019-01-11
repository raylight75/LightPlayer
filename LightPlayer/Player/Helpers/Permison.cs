using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.Threading.Tasks;
using Xamarin.Forms;
using Permission = Plugin.Permissions.Abstractions.Permission;

namespace Player.Helpers
{
    class Permision
    {
        public static bool CanExecute(object p)
        {
            return true;
        }
        
        public static async Task CheckPermission()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            if (status != PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                {
                    await Application.Current.MainPage.DisplayAlert("Request Permission", "Request Permission for folders", "OK");
                }
                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);                
                if (results.ContainsKey(Permission.Storage))
                    status = results[Permission.Storage];
            }            
        }        
    }
}
