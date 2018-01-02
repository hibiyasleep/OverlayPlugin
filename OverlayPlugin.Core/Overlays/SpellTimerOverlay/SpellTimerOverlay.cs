using Advanced_Combat_Tracker;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RainbowMage.OverlayPlugin.Overlays
{
    public class SpellTimerOverlay : OverlayBase<SpellTimerOverlayConfig>
    {
        IList<SerializableTimerFrameEntry> activatedTimers;

        public SpellTimerOverlay(SpellTimerOverlayConfig config)
            : base(config, config.Name)
        {
            this.activatedTimers = new List<SerializableTimerFrameEntry>();

            ActGlobals.oFormSpellTimers.OnSpellTimerNotify += (t) =>
            {
                lock (this.activatedTimers)
                {
                    var timerFrame = activatedTimers.Where(x => x.Original == t).FirstOrDefault();
                    if (timerFrame == null)
                    {
                        timerFrame = new SerializableTimerFrameEntry(t);
                        this.activatedTimers.Add(timerFrame);
                    }
                    else
                    {
                        timerFrame.Update(t);
                    }
                    foreach (var spellTimer in t.SpellTimers)
                    {
                        var timer = timerFrame.SpellTimers.Where(x => x.Original == spellTimer).FirstOrDefault();
                        if (timer == null)
                        {
                            timer = new SerializableSpellTimerEntry(spellTimer);
                            timerFrame.SpellTimers.Add(timer);
                        }
                    }
                }
            };
            ActGlobals.oFormSpellTimers.OnSpellTimerRemoved += (t) =>
            {
                //activatedTimers.Remove(t);
            };
        }

        protected override void Update()
        {
            this.Update(false);
        }

        protected override void FastUpdate()
        {
            this.Update(true);
        }

        private void Update(bool fastUpdate)
        {
            try
            {
                var updateScript = fastUpdate && this.m_latestJson != null ? this.m_latestJson : CreateEventDispatcherScript();

                if (this.Overlay != null &&
                    this.Overlay.Renderer != null &&
                    this.Overlay.Renderer.Browser != null)
                {
                    this.Overlay.Renderer.ExecuteScript(updateScript);
                }

            }
            catch (Exception ex)
            {
                Log(LogLevel.Error, "Update: {1}", this.Name, ex);
            }
        }

        private void RemoveExpiredEntries()
        {
            var expiredTimerFrames = new List<SerializableTimerFrameEntry>();
            foreach (var timerFrame in activatedTimers)
            {
                var expiredSpellTimers = new List<SerializableSpellTimerEntry>();
                bool expired = true;
                foreach (var timer in timerFrame.SpellTimers)
                {
                    if (timerFrame.StartCount - timerFrame.ExpireCount > (DateTime.Now - timer.StartTime).TotalSeconds)
                    {
                        expired = false;
                        break;
                    }
                    else
                    {
                        expiredSpellTimers.Add(timer);
                    }
                }
                if (expired)
                {
                    expiredTimerFrames.Add(timerFrame);
                }
                else
                {
                    foreach (var expiredSpellTimer in expiredSpellTimers)
                    {
                        timerFrame.SpellTimers.Remove(expiredSpellTimer);
                    }
                }
            }
            foreach (var expiredTimerFrame in expiredTimerFrames)
            {
                activatedTimers.Remove(expiredTimerFrame);
            }
        }

        internal string CreateJsonData()
        {
            string result;
            lock (this.activatedTimers)
            {
                RemoveExpiredEntries();
                result = JsonConvert.SerializeObject(this.activatedTimers);
            }

            if (!string.IsNullOrWhiteSpace(result))
            {
                return string.Format(
                    "{{ timerFrames: {0} }}",
                    result);
            }
            else
            {
                return "";
            }
        }
        
        private string m_latestJson;
        private string CreateEventDispatcherScript()
        {
            return this.m_latestJson =  "var ActXiv = " + this.CreateJsonData() + ";\n" +
                   "document.dispatchEvent(new CustomEvent('onOverlayDataUpdate', ActXiv));";
        }
    }
}
