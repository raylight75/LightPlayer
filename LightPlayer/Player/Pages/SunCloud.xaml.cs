using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Player.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SunCloud : ContentPage
	{
        public SunCloud ()
		{
			InitializeComponent ();
            SizeChanged += OnSizeChanged;
        }

        public void OnSizeChanged(object sender, EventArgs e)
        {
            if (Width > Height)
            {
                innerGrid.RowDefinitions.Clear();
                innerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                innerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                innerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                innerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                innerGrid.Children.Remove(header);
                innerGrid.Children.Add(header, 0, 0);
                header.BackgroundColor = Color.Transparent;
                innerGrid.Children.Remove(listView);
                innerGrid.Children.Add(listView, 0, 1);
                innerGrid.Children.Remove(innerStack);
                innerGrid.Children.Add(innerStack, 0, 2);
                BackgroundImage = "bg.png";
            }
            else
            {
                innerGrid.RowDefinitions.Clear();
                innerGrid.ColumnDefinitions.Clear();
                innerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                innerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                innerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                innerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                innerGrid.Children.Remove(header);
                innerGrid.Children.Add(header, 0, 0);
                header.BackgroundColor = Color.FromRgb(100, 41, 102);
                innerGrid.Children.Remove(listView);
                innerGrid.Children.Add(listView, 0, 2);
                innerGrid.Children.Remove(innerStack);
                innerGrid.Children.Add(innerStack, 0, 3);
                BackgroundImage = "backgroundbg.png";
            }
        }
    }
}