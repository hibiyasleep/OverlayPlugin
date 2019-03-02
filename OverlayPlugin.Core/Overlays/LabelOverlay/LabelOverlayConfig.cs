﻿using System;
using System.Xml.Serialization;

namespace RainbowMage.OverlayPlugin.Overlays
{
    [Serializable]
    public class LabelOverlayConfig : OverlayConfigBase
    {
        private string text;
        [XmlElement("Text")]
        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                if (this.text != value)
                {
                    this.text = value;
                    TextChanged?.Invoke(this, new TextChangedEventArgs(this.text));
                }
            }
        }

        private bool htmlModeEnabled;
        [XmlElement("HTMLModeEnabled")]
        public bool HtmlModeEnabled
        {
            get
            {
                return this.htmlModeEnabled;
            }
            set
            {
                if (this.htmlModeEnabled != value)
                {
                    this.htmlModeEnabled = value;
                    HTMLModeChanged?.Invoke(this, new StateChangedEventArgs<bool>(this.htmlModeEnabled));
                }
            }
        }

        public event EventHandler<TextChangedEventArgs> TextChanged;
        public event EventHandler<StateChangedEventArgs<bool>> HTMLModeChanged;

        public LabelOverlayConfig(string name)
            : base(name)
        {
            this.Text = "";
            this.HtmlModeEnabled = false;
        }

        // XmlSerializer用
        private LabelOverlayConfig()
            : base(null)
        {

        }

        public override Type OverlayType
        {
            get { return typeof(LabelOverlay); }
        }
    }
}
