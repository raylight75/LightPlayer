using Android.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Player.Interfaces
{
    public interface IMediaService
    {
        string Album { get; set; }
        string Artist { get; set; }
        string Genre { get; set; }
        string Duration { get; set; }
        Bitmap Image { get; set; }
        void GetMetadata(string file);
    }
}
