using Xamarin.Forms;

namespace Player.Models
{
    class Album
    {
        public string Title { get; set; }
        public ImageSource Image { get; set; }        

        public Album(string title, ImageSource image)
        {            
            Title = title;
            Image = image;
        }
    }
}
