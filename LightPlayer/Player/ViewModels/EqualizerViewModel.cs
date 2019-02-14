using Player.Commands;
using Player.Helpers;
using Player.Interfaces;
using Player.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Player.ViewModels
{
    class EqualizerViewModel : BaseViewModel
    {
        private Mediator _mediator;
        public ICommand BandChangedCommand { get; set; }
        public ICommand EqualizerChangedCommand { get; set; }
        private List<string> _bands;
        private List<Bands> _equalizer;

        public List<Bands> Equalizers
        {
            get { return _equalizer; }
            set
            {
                if (Equals(value, _equalizer)) return;
                _equalizer = value;
                OnPropertyChanged(nameof(Equalizers));
            }
        }

        public List<string> Bands
        {
            get { return _bands; }
            set
            {
                if (Equals(value, _bands)) return;
                _bands = value;
                OnPropertyChanged(nameof(Bands));
            }
        }

        public EqualizerViewModel(Mediator mediator)
        {
            _mediator = mediator;
            _equalizers = DependencyService.Get<IEqualizerSevice>();
            BandSelected = "Normal";
            Equalizers = new BandList();
            LoadCommands();
        }

        private void LoadCommands()
        {
            EqualizerChangedCommand = new RelayCommand(EqualizerChanged, Permision.CanExecute);
            BandChangedCommand = new RelayCommand(async parameter => { await BandChanged(); }, Permision.CanExecute);
            PlayingSelectedCommand = new RelayCommand(async parameter => { await PlayingSelected(); }, Permision.CanExecute);
        }

        public void SetEqualizers()
        {
            Equalizers = _equalizers.SetEqualizer(0);
            Bands = _equalizers.SetBands();
        }

        private void EqualizerChanged(object p)
        {
            int param = Convert.ToInt32(p);
            var result = Equalizers.FirstOrDefault(x => x.BandId == param);
            int value = result.Value;
            _equalizers.SetBandLevel(param, value);
        }

        private async Task BandChanged()
        {
            if (Bands == null)
            {
                await Application.Current.MainPage.DisplayAlert("Alert", "Play track first", "OK");
            }
            else
            {
                var action = await Application.Current.MainPage.DisplayActionSheet("Select Preset", "Cancel", null, Bands.ToArray());
                int idx = Bands.IndexOf(action.ToString());
                Equalizers = (action == "Cancel") ? _equalizers.SetEqualizer(0) : _equalizers.SetEqualizer(idx);
                BandSelected = (action == "Cancel") ? "Normal" : action.ToString();
            }
        }

        private async Task PlayingSelected()
        {
            await _mediator.Playing();
        }
    }
}
