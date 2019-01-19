using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Player.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Songs : ContentPage
    {
        private double width = 0;
        private double height = 0;

        public Songs()
        {
            InitializeComponent();
            SizeChanged += OnSizeChanged;
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

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (width != this.width || height != this.height)
            {
                this.width = width;
                this.height = height;
                if (width > height)
                {
                    outerStack.Orientation = StackOrientation.Vertical;
                    innerStack1.HeightRequest = 0;
                    innerStack2.HorizontalOptions = LayoutOptions.Center;                    
                }
                else
                {
                    innerStack1.HeightRequest = 160;
                }
            }
        }

        public void OnSizeChanged(object sender, EventArgs e)
        {
            background.Source = ImageSource.FromFile(Height > Width ? "background1.png" : "bg.png");
        }

    }
}