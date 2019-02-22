using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Player.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Equalizers : ContentPage
	{
        public Equalizers()
        {
            InitializeComponent();
            SizeChanged += OnSizeChanged;
        }

        public void OnSizeChanged(object sender, EventArgs e)
        {
            if (Width > Height)
            {
                innerGrid.RowDefinitions.Clear();
                innerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                innerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                innerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                innerGrid.Children.Remove(equalizers);
                innerGrid.Children.Add(equalizers, 0, 0);
                innerGrid.Children.Remove(innerStack);
                innerGrid.Children.Add(innerStack, 0, 1);
                BackgroundImage = "bg.png";
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
                BackgroundImage = "backgroundbg.png";
            }
        }
    }
}