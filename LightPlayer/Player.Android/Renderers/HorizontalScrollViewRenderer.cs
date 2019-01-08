using System;
using System.ComponentModel;
using Android.Content;
using Player.Controls;
using Player.Droid.Services.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(HorizontalScrollView), typeof(HorizontalScrollViewRenderer))]
namespace Player.Droid.Services.Renderers
{
    public class HorizontalScrollViewRenderer : ScrollViewRenderer
    {
        public HorizontalScrollViewRenderer(Context context) : base(context)
        {
            
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            var element = e.NewElement as HorizontalScrollView;
            element?.Render();

            if (e.OldElement != null || this.Element == null)
                return;

            if (e.OldElement != null)
                e.OldElement.PropertyChanged -= OnElementPropertyChanged;

            //e.NewElement.PropertyChanged += OnElementPropertyChanged;
        }

        protected void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (ChildCount > 0)
            {
                GetChildAt(0).HorizontalScrollBarEnabled = false;
                GetChildAt(0).VerticalScrollBarEnabled = false;
            }
        }
    }
}
