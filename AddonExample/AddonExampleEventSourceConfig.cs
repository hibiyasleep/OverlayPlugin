using Newtonsoft.Json.Linq;
using RainbowMage.OverlayPlugin;
using System;

namespace AddonExample
{
    [Serializable]
    public class AddonExampleEventSourceConfig
    {
        public string ExampleString = "Example String";
        public AddonExampleEventSourceConfig()
        {

        }

        public static AddonExampleEventSourceConfig LoadConfig(IPluginConfig pluginConfig)
        {
            var result = new AddonExampleEventSourceConfig();
            if (pluginConfig.EventSourceConfigs.ContainsKey("AddonExampleESConfig"))
            {
                var obj = pluginConfig.EventSourceConfigs["AddonExampleESConfig"];

                if (obj.TryGetValue("ExampleString", out JToken value))
                {
                    result.ExampleString = value.ToString();
                }
            }
            return result;
        }

        public void SaveConfig(IPluginConfig pluginConfig)
        {
            pluginConfig.EventSourceConfigs["AddonExampleESConfig"] = JObject.FromObject(this);
        }
    }
}
