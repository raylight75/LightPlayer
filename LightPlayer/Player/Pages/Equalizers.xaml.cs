using Player.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Player.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Equalizers : ContentPage
	{
		public Equalizers ()
		{
			InitializeComponent ();
            BindingContext = new EqualizerViewModel();
        }
	}
}