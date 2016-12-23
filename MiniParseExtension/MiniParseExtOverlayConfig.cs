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
