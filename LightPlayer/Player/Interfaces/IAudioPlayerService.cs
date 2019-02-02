using System;

namespace Player.Interfaces
{
    public interface IAudioPlayerService
    {
        void Play(string pathToAudioFile);
        void Play();
        void Pause();                
        void Seek(int value);        
        int SliderMax();
        int Position();
        Action OnFinishedPlaying { get; set; }        
    }
}
