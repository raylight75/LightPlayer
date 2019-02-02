using Player.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Player.Interfaces
{
    public interface IEqualizerSevice
    {
        List<Bands> SetEqualizer(int preset);
        List<string> SetBands();
        void SetBandLevel(int index, int progress);
    }
}
