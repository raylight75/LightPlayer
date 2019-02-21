using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Player.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Equalizers : ContentPage
	{
        private double width = 0;
        private double height = 0;

        public Equalizers()
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
                    innerGrid.RowDefinitions.Clear();                    
                    innerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    innerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                    innerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });                    
                    innerGrid.Children.Remove(equalizers);
                    innerGrid.Children.Add(equalizers, 0, 0);
                    innerGrid.Children.Remove(innerStack);
                    innerGrid.Children.Add(innerStack, 0, 1);
                }
                else
                {
                    innerGrid.RowDefinitions.Clear();
                    innerGrid.ColumnDefinitions.Clear();
                    innerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    innerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });                    
                    innerGrid.Children.Remove(equalizers);
                    innerGrid.Children.Add(equalizers, 0, 0);
                    innerGrid.Children.Remove(innerStack);
                    innerGrid.Children.Add(innerStack, 0, 1);

                }
            }
        }

        public void OnSizeChanged(object sender, EventArgs e)
        {
            BackgroundImage = (Height > Width ? "backgroundbg.png" : "bg.png");
        }
    }
}