using System;
using System.IO;
using System.Xml.Serialization;

namespace RainbowMage.OverlayPlugin.Overlays
{
    [Serializable]
    public class MiniParseExtOverlayConfig : OverlayConfigBase
    {
        public event EventHandler<SortKeyChangedEventArgs> SortKeyChanged;
        public event EventHandler<SortTypeChangedEventArgs> SortTypeChanged;
        public event EventHandler<ScrPathEventArgs> ScrPathChanged;
        public event EventHandler<ShowDebugLogEventArgs> ShowDebugLogChanged;
        public event EventHandler<EnableOnLogLineReadEventArgs> EnableOnLogLineReadChanged;
        public event EventHandler<EnableBeforeLogLineReadEventArgs> enableBeforeLogLineReadChanged;

        private bool enableBeforeLogLineRead;
        [XmlElement("EnableOnLogLineRead")]
        public bool EnableBeforeLogLineRead
        {
            get
            {
                return enableBeforeLogLineRead;
            }
            set
            {
                if (enableBeforeLogLineRead != value)
                {
                    enableBeforeLogLineRead = value;
                    if (enableBeforeLogLineReadChanged != null)
                    {
                        enableBeforeLogLineReadChanged(this,
                            new EnableBeforeLogLineReadEventArgs(enableBeforeLogLineRead));
                    }
                }
            }
        }

        private bool enableOnLogLineRead;
        [XmlElement("EnableOnLogLineRead")]
        public bool EnableOnLogLineRead
        {
            get
            {
                return enableOnLogLineRead;
            }
            set
            {
                if (enableOnLogLineRead != value)
                {
                    enableOnLogLineRead = value;
                    if (EnableOnLogLineReadChanged != null)
                    {
                        EnableOnLogLineReadChanged(this, 
                            new EnableOnLogLineReadEventArgs(enableOnLogLineRead));
                    }
                }
            }
        }

        private bool showDebugLog;
        [XmlElement("ShowDebugLog")]
        public bool ShowDebugLog
        {
            get
            {
                return showDebugLog;
            }
            set
            {
                if (showDebugLog != value)
                {
                    showDebugLog = value;
                    if (ScrPathChanged != null)
                    {
                        ShowDebugLogChanged(this, 
                            new ShowDebugLogEventArgs(showDebugLog));
                    }
                }
            }
        }

        private string scrPath;
        [XmlElement("ScrPath")]
        public string ScrPath
        {
            get
            {
                return scrPath;
            }
            set
            {
                if (scrPath != value)
                {
                    scrPath = value;
                    if (ScrPathChanged != null)
                    {
                        ScrPathChanged(this, new ScrPathEventArgs(scrPath));
                    }
                }
            }
        }

        private string sortKey;
        [XmlElement("SortKey")]
        public string SortKey
        {
            get
            {
                return sortKey;
            }
            set
            {
                if (sortKey != value)
                {
                    sortKey = value;
                    if (SortKeyChanged != null)
                    {
                        SortKeyChanged(this, new SortKeyChangedEventArgs(sortKey));
                    }
                }
            }
        }

        private MiniParseSortType sortType;
        [XmlElement("SortType")]
        public MiniParseSortType SortType
        {
            get
            {
                return sortType;
            }
            set
            {
                if (sortType != value)
                {
                    sortType = value;
                    if (SortTypeChanged != null)
                    {
                        SortTypeChanged(this, new SortTypeChangedEventArgs(sortType));
                    }
                }
            }
        }

        public MiniParseExtOverlayConfig(string name) : base(name)
        {
            enableBeforeLogLineRead = false;
            enableOnLogLineRead = false;
            showDebugLog = false;
            scrPath = Environment.CurrentDirectory + "\\scr";
            sortKey = "encdps";
            sortType = MiniParseSortType.NumericDescending;
        }

        // XmlSerializer用
        private MiniParseExtOverlayConfig() : base(null)
        {

        }

        public override Type OverlayType
        {
            get { return typeof(MiniParseExtOverlay); }
        }
    }
}
