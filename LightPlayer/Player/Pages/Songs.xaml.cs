using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Player.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Songs : ContentPage
    {
        public Songs()
        {
            InitializeComponent();
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            listView.ScrollTo(listView.SelectedItem, ScrollToPosition.Center, true);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            listView.ScrollTo(listView.SelectedItem, ScrollToPosition.Center, true);
        }
    }
}