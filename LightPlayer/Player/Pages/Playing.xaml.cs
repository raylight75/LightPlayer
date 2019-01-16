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
    }
}