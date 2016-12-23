using Advanced_Combat_Tracker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Diagnostics;

namespace RainbowMage.OverlayPlugin.Overlays
{
    public class MiniParseExtOverlay : OverlayBase<MiniParseExtOverlayConfig>
    {
        private string prevEncounterId { get; set; }
        private DateTime prevEndDateTime { get; set; }
        private bool prevEncounterActive { get; set; }

        private static string updateStringCache = "";
        private static DateTime updateStringCacheLastUpdate;
        private static readonly TimeSpan updateStringCacheExpireInterval = new TimeSpan(0, 0, 0, 0, 500); // 500 msec

        public MiniParseExtOverlay(MiniParseExtOverlayConfig config)
            : base(config, config.Name)
        {
            ActGlobals.oFormActMain.BeforeLogLineRead += (o, e) =>
            {
                Overlay.Renderer.ExecuteScript("document.dispatchEvent(new CustomEvent('beforeLogLineRead', { detail: \""+ Utility.CreateJsonSafeString(e.logLine)+ "\" }));");
            };
        }

        public override void Navigate(string url)
        {
            base.Navigate(url);

            this.prevEncounterId = null;
            this.prevEndDateTime = DateTime.MinValue;
        }

        protected override void Update()
        {
            if (CheckIsActReady())
            {
                // 最終更新時刻に変化がないなら更新を行わない
                if (this.prevEncounterId == ActGlobals.oFormActMain.ActiveZone.ActiveEncounter.EncId &&
                    this.prevEndDateTime == ActGlobals.oFormActMain.ActiveZone.ActiveEncounter.EndTime &&
                    this.prevEncounterActive == ActGlobals.oFormActMain.ActiveZone.ActiveEncounter.Active)
                {
                    return;
                }

                this.prevEncounterId = ActGlobals.oFormActMain.ActiveZone.ActiveEncounter.EncId;
                this.prevEndDateTime = ActGlobals.oFormActMain.ActiveZone.ActiveEncounter.EndTime;
                this.prevEncounterActive = ActGlobals.oFormActMain.ActiveZone.ActiveEncounter.Active;

                var updateScript = CreateEventDispatcherScript();

                if (this.Overlay != null &&
                    this.Overlay.Renderer != null &&
                    this.Overlay.Renderer.Browser != null)
                {
                    this.Overlay.Renderer.ExecuteScript(updateScript);
                }
            }
        }

        private string CreateEventDispatcherScript()
        {
            // why?
            return "var ActXiv = " + CreateJsonData() + ";\n" +
                   "document.dispatchEvent(new CustomEvent('onOverlayDataUpdate', { detail: ActXiv }));";
        }

        protected override void Log(LogLevel level, string message)
        {
            if(message.StartsWith("BrowserConsole: "))
            {
                string s = message.Replace("BrowserConsole: ", "");

                if (s.StartsWith("EncounterEnd"))
                {
                    level = LogLevel.Debug;
                    ActGlobals.oFormActMain.EndCombat(true);

                    Overlay.Renderer.ExecuteScript("document.dispatchEvent(new CustomEvent('onEncounterEndExecuted', { detail:{take: true}}));");
                }
                else if (s.StartsWith("CaptureOverlay"))
                {
                    level = LogLevel.Debug;
                    Bitmap bmp = new Bitmap(Overlay.Size.Width, Overlay.Size.Height);
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.CopyFromScreen(Overlay.Location.X, Overlay.Location.Y, 0, 0, Overlay.Size, CopyPixelOperation.SourceCopy);
                    }

                    Directory.CreateDirectory(Environment.CurrentDirectory + "\\scr\\");
                    bmp.Save(Environment.CurrentDirectory + "\\scr\\" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png");

                    Overlay.Renderer.ExecuteScript("document.dispatchEvent(new CustomEvent('onScreenShotTaked', { detail:{take: true, fileurl:\""+ Utility.CreateJsonSafeString((Environment.CurrentDirectory + "\\scr\\")) + "\" }}));");
                }
                else if (s.StartsWith("Isloaded"))
                {
                    level = LogLevel.Debug;

                    Overlay.Renderer.ExecuteScript("var srcEnable = true; var endEnable = true; var zoomEnable = true; var version = '1.1';");
                }
                else if (s.StartsWith("Zoom"))
                {
                    level = LogLevel.Debug;
                    string sv = s.Replace("Zoom", "").Substring(0, 2);
                    string zoomsize = "75";
                    switch (sv)
                    {
                        case "75":
                            Overlay.Renderer.Browser.GetHost().SetZoomLevel(-1.25);
                            zoomsize = "-1.25";
                            break;
                        case "80":
                            Overlay.Renderer.Browser.GetHost().SetZoomLevel(-1.2);
                            zoomsize = "-1.2";
                            break;
                        case "90":
                            Overlay.Renderer.Browser.GetHost().SetZoomLevel(-1.1);
                            zoomsize = "-1.1";
                            break;
                        case "10":
                            Overlay.Renderer.Browser.GetHost().SetZoomLevel(0);
                            zoomsize = "1.0";
                            break;
                        case "11":
                            Overlay.Renderer.Browser.GetHost().SetZoomLevel(1.1);
                            zoomsize = "1.1";
                            break;
                        case "12":
                            Overlay.Renderer.Browser.GetHost().SetZoomLevel(1.25);
                            zoomsize = "1.25";
                            break;
                        case "15":
                            Overlay.Renderer.Browser.GetHost().SetZoomLevel(1.5);
                            zoomsize = "1.5";
                            break;
                    }

                    Overlay.Renderer.ExecuteScript("document.dispatchEvent(new CustomEvent('onZoomSizeChanged', { detail:{ take: true, size:\"" + zoomsize + "\" }}));");
                }
            }

            base.Log(level, message);
        }

        internal string CreateJsonData()
        {
            if (DateTime.Now - updateStringCacheLastUpdate < updateStringCacheExpireInterval)
            {
                return updateStringCache;
            }

            if (!CheckIsActReady())
            {
                return "{}";
            }

#if DEBUG
            var stopwatch = new Stopwatch();
            stopwatch.Start();
#endif

            var allies = ActGlobals.oFormActMain.ActiveZone.ActiveEncounter.GetAllies();
            Dictionary<string, string> encounter = null;
            List<KeyValuePair<CombatantData, Dictionary<string, string>>> combatant = null;

            var encounterTask = Task.Run(() =>
                {
                    encounter = GetEncounterDictionary(allies);
                });
            var combatantTask = Task.Run(() =>
                {
                    combatant = GetCombatantList(allies);
                    SortCombatantList(combatant);
                });
            Task.WaitAll(encounterTask, combatantTask);

            var builder = new StringBuilder();
            builder.Append("{");
            builder.Append("\"Encounter\": {");
            var isFirst1 = true;
            foreach (var pair in encounter)
            {
                if (isFirst1)
                {
                    isFirst1 = false;
                }
                else
                {
                    builder.Append(",");
                }
                var valueString = Utility.CreateJsonSafeString(Utility.ReplaceNaNString(pair.Value, "---"));
                builder.AppendFormat("\"{0}\":\"{1}\"", Utility.CreateJsonSafeString(pair.Key), valueString);
            }
            builder.Append("},");
            builder.Append("\"Combatant\": {");
            var isFirst2 = true;
            foreach (var pair in combatant)
            {
                if (isFirst2)
                {
                    isFirst2 = false;
                }
                else
                {
                    builder.Append(",");
                }
                builder.AppendFormat("\"{0}\": {{", Utility.CreateJsonSafeString(pair.Key.Name));
                var isFirst3 = true;
                foreach (var pair2 in pair.Value)
                {
                    if (isFirst3)
                    {
                        isFirst3 = false;
                    }
                    else
                    {
                        builder.Append(",");
                    }
                    var valueString = Utility.CreateJsonSafeString(Utility.ReplaceNaNString(pair2.Value, "---"));
                    builder.AppendFormat("\"{0}\":\"{1}\"", Utility.CreateJsonSafeString(pair2.Key), valueString);
                }
                builder.Append("}");
            }
            builder.Append("},");
            builder.AppendFormat("\"isActive\": {0}", ActGlobals.oFormActMain.ActiveZone.ActiveEncounter.Active ? "true" : "false");
            builder.Append("}");

#if DEBUG
            stopwatch.Stop();
            Log(LogLevel.Trace, "CreateUpdateScript: {0} msec", stopwatch.Elapsed.TotalMilliseconds);
#endif

            var result = builder.ToString();
            updateStringCache = result;
            updateStringCacheLastUpdate = DateTime.Now;

            return result;
        }

        private void SortCombatantList(List<KeyValuePair<CombatantData, Dictionary<string, string>>> combatant)
        {
            // 数値で並び替え
            if (this.Config.SortType == MiniParseSortType.NumericAscending ||
                this.Config.SortType == MiniParseSortType.NumericDescending)
            {
                combatant.Sort((x, y) =>
                {
                    int result = 0;
                    if (x.Value.ContainsKey(this.Config.SortKey) &&
                        y.Value.ContainsKey(this.Config.SortKey))
                    {
                        double xValue, yValue;
                        double.TryParse(x.Value[this.Config.SortKey].Replace("%", ""), out xValue);
                        double.TryParse(y.Value[this.Config.SortKey].Replace("%", ""), out yValue);

                        result = xValue.CompareTo(yValue);

                        if (this.Config.SortType == MiniParseSortType.NumericDescending)
                        {
                            result *= -1;
                        }
                    }

                    return result;
                });
            }
            // 文字列で並び替え
            else if (
                this.Config.SortType == MiniParseSortType.StringAscending ||
                this.Config.SortType == MiniParseSortType.StringDescending)
            {
                combatant.Sort((x, y) =>
                {
                    int result = 0;
                    if (x.Value.ContainsKey(this.Config.SortKey) &&
                        y.Value.ContainsKey(this.Config.SortKey))
                    {
                        result = x.Value[this.Config.SortKey].CompareTo(y.Value[this.Config.SortKey]);

                        if (this.Config.SortType == MiniParseSortType.StringDescending)
                        {
                            result *= -1;
                        }
                    }

                    return result;
                });
            }
        }

        private List<KeyValuePair<CombatantData, Dictionary<string, string>>> GetCombatantList(List<CombatantData> allies)
        {
#if DEBUG
            var stopwatch = new Stopwatch();
            stopwatch.Start();
#endif

            var combatantList = new List<KeyValuePair<CombatantData, Dictionary<string, string>>>();
            Parallel.ForEach(allies, (ally) =>
            //foreach (var ally in allies)
            {
                var valueDict = new Dictionary<string, string>();
                foreach (var exportValuePair in CombatantData.ExportVariables)
                {
                    try
                    {
                        // NAME タグには {NAME:8} のようにコロンで区切られたエクストラ情報が必要で、
                        // プラグインの仕組み的に対応することができないので除外する
                        if (exportValuePair.Key == "NAME")
                        {
                            continue;
                        }

                        // ACT_FFXIV_Plugin が提供する LastXXDPS は、
                        // ally.Items[CombatantData.DamageTypeDataOutgoingDamage].Items に All キーが存在しない場合に、
                        // プラグイン内で例外が発生してしまい、パフォーマンスが悪化するので代わりに空の文字列を挿入する
                        if (exportValuePair.Key == "Last10DPS" ||
                            exportValuePair.Key == "Last30DPS" ||
                            exportValuePair.Key == "Last60DPS")
                        {
                            if (!ally.Items[CombatantData.DamageTypeDataOutgoingDamage].Items.ContainsKey("All"))
                            {
                                valueDict.Add(exportValuePair.Key, "");
                                continue;
                            }
                        }

                        var value = exportValuePair.Value.GetExportString(ally, "");
                        valueDict.Add(exportValuePair.Key, value);
                    }
                    catch (Exception e)
                    {
                        Log(LogLevel.Debug, "GetCombatantList: {0}: {1}: {2}", ally.Name, exportValuePair.Key, e);
                        continue;
                    }
                }

                lock (combatantList)
                {
                    combatantList.Add(new KeyValuePair<CombatantData, Dictionary<string, string>>(ally, valueDict));
                }
            }
            );

#if DEBUG
            stopwatch.Stop();
            Log(LogLevel.Trace, "GetCombatantList: {0} msec", stopwatch.Elapsed.TotalMilliseconds);
#endif

            return combatantList;
        }

        private Dictionary<string, string> GetEncounterDictionary(List<CombatantData> allies)
        {
#if DEBUG
            var stopwatch = new Stopwatch();
            stopwatch.Start();
#endif

            var encounterDict = new Dictionary<string, string>();
            //Parallel.ForEach(EncounterData.ExportVariables, (exportValuePair) =>
            foreach (var exportValuePair in EncounterData.ExportVariables)
            {
                try
                {
                    // ACT_FFXIV_Plugin が提供する LastXXDPS は、
                    // ally.Items[CombatantData.DamageTypeDataOutgoingDamage].Items に All キーが存在しない場合に、
                    // プラグイン内で例外が発生してしまい、パフォーマンスが悪化するので代わりに空の文字列を挿入する
                    if (exportValuePair.Key == "Last10DPS" ||
                        exportValuePair.Key == "Last30DPS" ||
                        exportValuePair.Key == "Last60DPS")
                    {
                        if (!allies.All((ally) => ally.Items[CombatantData.DamageTypeDataOutgoingDamage].Items.ContainsKey("All")))
                        {
                            encounterDict.Add(exportValuePair.Key, "");
                            continue;
                        }
                    }

                    var value = exportValuePair.Value.GetExportString(
                        ActGlobals.oFormActMain.ActiveZone.ActiveEncounter,
                        allies,
                        "");
                    //lock (encounterDict)
                    //{
                        encounterDict.Add(exportValuePair.Key, value);
                    //}
                }
                catch (Exception e)
                {
                    Log(LogLevel.Debug, "GetEncounterDictionary: {0}: {1}", exportValuePair.Key, e);
                }
            }
            //);

#if DEBUG
            stopwatch.Stop();
            Log(LogLevel.Trace, "GetEncounterDictionary: {0} msec", stopwatch.Elapsed.TotalMilliseconds);
#endif

            return encounterDict;
        }

        private static bool CheckIsActReady()
        {
            if (ActGlobals.oFormActMain != null &&
                ActGlobals.oFormActMain.ActiveZone != null &&
                ActGlobals.oFormActMain.ActiveZone.ActiveEncounter != null &&
                EncounterData.ExportVariables != null &&
                CombatantData.ExportVariables != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
