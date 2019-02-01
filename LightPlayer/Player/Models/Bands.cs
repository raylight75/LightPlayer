namespace Player.Models
{
    public class Bands
    {        
        public string Frequency { get; set; }
        public int BandId { get; set; }
        public int Value { get; set; }
        public int MaxValue { get; set; }

        public Bands(string frequency, int bandId, int value, int maxValue)
        {           
            Frequency = frequency;
            BandId = bandId;
            Value = value;
            MaxValue = maxValue;
        }
    }
}
