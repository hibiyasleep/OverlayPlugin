using System;
using System.Xml.Serialization;

namespace RainbowMage.OverlayPlugin.Overlays
{
    [Serializable]
    public class MiniParseOverlayConfig : OverlayConfigBase
    {
        public event EventHandler<SortKeyChangedEventArgs> SortKeyChanged;
        public event EventHandler<SortTypeChangedEventArgs> SortTypeChanged;
        public event EventHandler<HidePlayerNameChangedEventArgs> HidePlayerNameChanged;
        public event EventHandler<UpdateIntervalChangedEventArgs> OverlayUpdateIntervalChanged;

        private string sortKey;
        [XmlElement("SortKey")]
        public string SortKey
        {
            get
            {
                return this.sortKey;
            }
            set
            {
                if (this.sortKey != value)
                {
                    this.sortKey = value;
                    SortKeyChanged?.Invoke(this, new SortKeyChangedEventArgs(this.sortKey));
                }
            }
        }

        private MiniParseSortType sortType;
        [XmlElement("SortType")]
        public MiniParseSortType SortType
        {
            get
            {
                return this.sortType;
            }
            set
            {
                if (this.sortType != value)
                {
                    this.sortType = value;
                    SortTypeChanged?.Invoke(this, new SortTypeChangedEventArgs(this.sortType));
                }
            }
        }

        private int overlayUpdateInterval = 1000;
        [XmlElement("OverlayUpdateInterval")]
        public int OverlayUpdateInterval
        {
            get
            {
                return this.overlayUpdateInterval;
            }
            set
            {
                if (this.overlayUpdateInterval != value)
                {
                    this.overlayUpdateInterval = value;
                    if (HidePlayerNameChanged != null)
                    {
                        OverlayUpdateIntervalChanged(this, new UpdateIntervalChangedEventArgs(value));
                    }
                }
            }
        }

        public MiniParseOverlayConfig(string name) : base(name)
        {
            this.sortKey = "encdps";
            this.sortType = MiniParseSortType.NumericDescending;
        }

        // XmlSerializer用
        private MiniParseOverlayConfig() : base(null)
        {

        }

        public override Type OverlayType
        {
            get { return typeof(MiniParseOverlay); }
        }
    }
}
