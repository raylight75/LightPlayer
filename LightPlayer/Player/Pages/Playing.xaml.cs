using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Player.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Playing : ContentPage
    {
        private double width = 0;
        private double height = 0;

        public Playing()
        {
            InitializeComponent();
            SizeChanged += OnSizeChanged;
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
                    albumArt.HeightRequest = 0;
                }
                else
                {
                    albumArt.HeightRequest = 210;
                }
            }
        }

        public void OnSizeChanged(object sender, EventArgs e)
        {
            BackgroundImage = (Height > Width ? "backgroundbg.png" : "bg.png");
        }
    }
}