using Advanced_Combat_Tracker;
using Newtonsoft.Json;
using System;

namespace RainbowMage.OverlayPlugin.Overlays
{
    [JsonObject]
    class SerializableSpellTimerEntry
    {
        public DateTime StartTime { get; set; }
        private static readonly DateTime EpochTime = new DateTime(1970, 1, 1);

        [JsonProperty(PropertyName = "startTime")]
        public long StartTimeLong
        {
            get
            {
                return (this.StartTime.Ticks - EpochTime.Ticks) / 10000;
            }
            set
            {
                this.StartTime = new DateTime(EpochTime.Ticks + value * 10000);
            }
        }

        public SpellTimer Original { get; private set; }

        public SerializableSpellTimerEntry(SpellTimer spellTimer)
        {
            this.StartTime = DateTime.Now;

            this.Original = spellTimer;
        }
    }
}
