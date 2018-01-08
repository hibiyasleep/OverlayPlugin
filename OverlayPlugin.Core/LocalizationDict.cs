using System.Collections.Generic;

namespace RainbowMage.OverlayPlugin
{
    class LocalizationDict
    {
        IDictionary<TextItem, IDictionary<string, string>> dict;

        public LocalizationDict()
        {
            this.dict = new Dictionary<TextItem, IDictionary<string, string>>();
        }

        public string this[TextItem item, string locale]
        {
            get
            {
                if (dict.ContainsKey(item))
                {
                    if (dict[item].ContainsKey(locale))
                    {
                        return dict[item][locale];
                    }
                    else if (dict[item].ContainsKey(""))
                    {
                        return dict[item][""];
                    }
                    else
                    {
                        throw new KeyNotFoundException();
                    }
                }
                throw new KeyNotFoundException();
            }
            set
            {
                if (!dict.ContainsKey(item))
                {
                    dict.Add(item, new Dictionary<string, string>());
                }
                if (!dict[item].ContainsKey(locale))
                {
                    dict[item].Add(locale, value);
                }
                else
                {
                    dict[item][locale] = value;
                }
            }
        }
    }
}
