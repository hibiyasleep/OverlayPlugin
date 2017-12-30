using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RainbowMage.OverlayPlugin.Overlays
{
    [Serializable]
    public class LogParseOverlayConfig : OverlayConfigBase
    {
        /*
        public event EventHandler<SortKeyChangedEventArgs> SortKeyChanged;
        public event EventHandler<SortTypeChangedEventArgs> SortTypeChanged;

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
                    if (SortKeyChanged != null)
                    {
                        SortKeyChanged(this, new SortKeyChangedEventArgs(this.sortKey));
                    }
                }
            }
        }

        private LogParseSortType sortType;
        [XmlElement("SortType")]
        public LogParseSortType SortType
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
                    if (SortTypeChanged != null)
                    {
                        SortTypeChanged(this, new SortTypeChangedEventArgs(this.sortType));
                    }
                }
            }
        }
        */
        public LogParseOverlayConfig(string name) : base(name)
        {
            // this.sortKey = "encdps";
            // this.sortType = LogParseSortType.NumericDescending;
        }

        // XmlSerializer用
        private LogParseOverlayConfig() : base(null)
        {

        }

        public override Type OverlayType
        {
            get { return typeof(LogParseOverlay); }
        }
    }
    /*
    public enum LogParseSortType
    {
        None,
        StringAscending,
        StringDescending,
        NumericAscending,
        NumericDescending
    }
    */
}
