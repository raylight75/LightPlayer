using Android.Graphics;
using Player.Models;
using System;
using System.Collections.Generic;

namespace Player.Interfaces
{
    public interface IAudioPlayerService
    {
        string Album { get; set; }
        string Artist { get; set; }
        string Genre { get; set; }
        string Duration { get; set; }

        void Play(string pathToAudioFile);
        void Play();
        void Pause();
        void Stop();
        void Reset();        
        void Seek(int value);       
        Bitmap GetImage(string file);
        void GetMetadata(string file);
        int SliderMax();
        int Position();
        Action OnFinishedPlaying { get; set; }
        List<Bands> SetEqualizer(int preset);
        List<string> SetBands();
        void SetBandLevel(int index, int progress);
    }
}
