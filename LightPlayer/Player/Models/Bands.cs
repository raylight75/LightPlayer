namespace Player.Models
{
    public class Bands
    {
        public string LowBand { get; set; }
        public string HighBand { get; set; }
        public string Frequency { get; set; }
        public int Value { get; set; }

        public Bands(string lowBand, string highBand, string frequency, int value)
        {
            LowBand = lowBand;
            HighBand = highBand;
            Frequency = frequency;
            Value = value;
        }
    }
}
