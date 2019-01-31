using Player.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace Player.ViewModels
{
    class EqualizerViewModel : INotifyPropertyChanged
    {
        private IAudioPlayerService _audioPlayer;
        private List<string> _frequency;
        private List<string> _presets;

        public List<string> Frequency
        {
            get { return _frequency; }
            set
            {
                if (Equals(value, _frequency)) return;
                _frequency = value;
                OnPropertyChanged(nameof(Frequency));
            }
        }

        public EqualizerViewModel()
        {
            _audioPlayer = DependencyService.Get<IAudioPlayerService>();
            SetFfrequency();
        }        

        private void SetFfrequency()
        {
            //Frequency = _audioPlayer.SetEqualizer();            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
