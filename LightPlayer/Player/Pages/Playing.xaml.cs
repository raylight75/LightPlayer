using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Player.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Playing : ContentPage
    {        

        public Playing()
        {
            InitializeComponent();
            SizeChanged += OnSizeChanged;
        }        

        public void OnSizeChanged(object sender, EventArgs e)
        {
            if (Width > Height)
            {
                outerStack.Orientation = StackOrientation.Vertical;
                innerGrid.Children.Remove(dummyStack);
                innerGrid.Children.Remove(albumArt);
                dummyStack.HeightRequest = 20;
                if(Device.Idiom == TargetIdiom.Tablet)
                {
                    albumArt.HeightRequest = 200;
                }
                else
                {
                    albumArt.HeightRequest = 0;
                }               
                BackgroundImage = "bg.png";
            }
            else
            {
                dummyStack.HeightRequest = 30;
                albumArt.Margin = 10;
                albumArt.HeightRequest = 210;
                BackgroundImage = "backgroundbg.png";
            }
        }
    }
}