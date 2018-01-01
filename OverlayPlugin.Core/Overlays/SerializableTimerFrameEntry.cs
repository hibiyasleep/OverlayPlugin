using Advanced_Combat_Tracker;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace RainbowMage.OverlayPlugin.Overlays
{
    [JsonObject]
    class SerializableTimerFrameEntry
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }
        [JsonProperty(PropertyName = "color")]
        public int Color { get; set; }
        [JsonProperty(PropertyName = "startCount")]
        public int StartCount { get; set; }
        [JsonProperty(PropertyName = "warningCount")]
        public int WarningCount { get; set; }
        [JsonProperty(PropertyName = "expireCount")]
        public int ExpireCount { get; set; }
        [JsonProperty(PropertyName = "tooltip")]
        public string Tooltip { get; set; }
        [JsonProperty(PropertyName = "absoluteTiming")]
        public bool AbsoluteTiming { get; set; }
        [JsonProperty(PropertyName = "onlyMasterTicks")]
        public bool OnlyMasterTicks { get; set; }
        [JsonProperty(PropertyName = "oneOnly")]
        public bool OneOnly { get; set; }
        //[JsonProperty(PropertyName = "masterExists")]
        //public bool MasterExists { get; set; }
        //[JsonProperty(PropertyName = "activeInList")]
        //public bool ActiveInList { get; set; }
        [JsonProperty(PropertyName = "spellTimers")]
        public IList<SerializableSpellTimerEntry> SpellTimers { get; set; }

        public TimerFrame Original { get; private set; }

        public SerializableTimerFrameEntry(TimerFrame timerFrame)
        {
            this.Update(timerFrame);

            this.SpellTimers = new List<SerializableSpellTimerEntry>();

            this.Original = timerFrame;
        }

        public void Update(TimerFrame timerFrame)
        {
            this.Name = timerFrame.Name;
            this.Key = timerFrame.TimerData.Key;
            this.Color = timerFrame.TimerData.FillColor.ToArgb();
            this.StartCount = timerFrame.TimerData.TimerValue;
            this.WarningCount = timerFrame.TimerData.WarningValue;
            this.ExpireCount = timerFrame.TimerData.RemoveValue;
            this.Tooltip = timerFrame.TimerData.Tooltip;
            this.OnlyMasterTicks = timerFrame.TimerData.OnlyMasterTicks;
            this.AbsoluteTiming = timerFrame.TimerData.AbsoluteTiming;
            //this.OneOnly = timerFrame.OneOnly;
            //this.MasterExists = timerFrame.MasterExists;

        }
    }

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
