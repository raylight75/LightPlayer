using Player.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Player.ViewModels
{
    class Mediator
    {
        public INavigationService _navigationService;
        public INavigation Navigation { get; set; }
        private static Mediator instance = null;
        private static readonly object padlock = new object();
        private EqualizerViewModel _eqm;
        private Page _playing;

        public EqualizerViewModel Equalizer
        {
            set { _eqm = value; }
        }

        public Page Page
        {
            set { _playing = value; }
        }

        private Mediator()
        {
            _navigationService = DependencyService.Get<INavigationService>();
            Navigation = _navigationService.Navigation;
        }

        public static Mediator Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Mediator();
                    }
                    return instance;
                }
            }
        }

        public void SetEqualizer()
        {
            _eqm.SetEqualizers();
        }

        public async Task Push(Page page)
        {
            await Navigation.PushAsync(page);
        }

        public async Task Playing()
        {
            await Navigation.PushModalAsync(_playing);
        }

    }
}
