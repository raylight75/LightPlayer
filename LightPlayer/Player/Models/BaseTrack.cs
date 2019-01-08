using System;

namespace Player.Models
{
    abstract class BaseTrack
    {
        public int Id { get; set; }
        public int Index { get; set; }
        public string FriendlyName { get; set; }
        public abstract string Duration { get; set; }
        public DateTime Created { get; set; }
    }
}
