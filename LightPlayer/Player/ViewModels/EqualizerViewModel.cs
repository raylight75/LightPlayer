using Player.Interfaces;
using Xamarin.Forms;

namespace Player.ViewModels
{
    class EqualizerViewModel
    {
        private IAudioPlayerService _audioPlayer;

        private void InitApp()
        {
            _audioPlayer = DependencyService.Get<IAudioPlayerService>();          
        }
    }
}
