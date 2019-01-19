using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Player.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SunCloud : ContentPage
	{
        private double width = 0;
        private double height = 0;

        public SunCloud ()
		{
			InitializeComponent ();
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
                    innerGrid.HeightRequest = 0;
                    innerStack2.HorizontalOptions = LayoutOptions.Center;
                }
                else
                {
                    innerGrid.HeightRequest = 100;
                }
            }
        }

        public void OnSizeChanged(object sender, EventArgs e)
        {
            background.Source = ImageSource.FromFile(Height > Width ? "background2.png" : "bg.png");
        }
    }
}