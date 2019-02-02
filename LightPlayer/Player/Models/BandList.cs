using System.Collections.Generic;

namespace Player.Models
{
    class BandList : List<Bands>
    {
        private int level = 1500;
        private int highLevel = 1500;

        public BandList() : base()
        {
            Add(new Bands("64Hz", 0, level, highLevel));
            Add(new Bands("230Hz", 1, level, highLevel));
            Add(new Bands("910Hz", 2, level, highLevel));
            Add(new Bands("9600Hz", 3, level, highLevel));
            Add(new Bands("14000Hz", 4, level, highLevel));
        }
    }
}
