using Advanced_Combat_Tracker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RainbowMage.OverlayPlugin
{
    public class ACTColumnAdder
    {
        public static void Init()
        {
            /* 
             * Part of ACTColumnAdder: 
             * https://github.com/laiglinne-ff/ACTColumnAdder/blob/master/ACTColumnAdder/Class1.cs
             * Copyright (c) 2017 Laighlinne; 
             * Licensed GPL-3.0 license.
             */
            string outD = CombatantData.DamageTypeDataOutgoingDamage;
            string outH = CombatantData.DamageTypeDataOutgoingHealing;

            if (!CombatantData.ExportVariables.ContainsKey("Last10DPS"))
                CombatantData.ExportVariables.Add("Last10DPS",
                    new CombatantData.TextExportFormatter(
                        "Last10DPS",
                        "Last 10 Seconds DPS",
                        "Average DPS for last 10 seconds.",
                        (Data, ExtraFormat) =>
                        (Data.Items[outD].Items["All"].Items.ToList().Where
                            (
                                x => x.Time >= ActGlobals.oFormActMain.LastKnownTime.Subtract(new TimeSpan(0, 0, 10))
                            ).Sum
                            (
                                x => x.Damage.Number
                            ) / (Data.Duration.TotalSeconds < 10.0 ? Data.Duration.TotalSeconds : 10.0)
                        ).ToString("0.00")
                    ));

            if (!CombatantData.ExportVariables.ContainsKey("Last30DPS"))
                CombatantData.ExportVariables.Add("Last30DPS",
                    new CombatantData.TextExportFormatter(
                        "Last30DPS",
                        "Last 30 Seconds DPS",
                        "Average DPS for last 30 seconds.",
                        (Data, ExtraFormat) =>
                        (Data.Items[outD].Items["All"].Items.ToList().Where
                            (
                                x => x.Time >= ActGlobals.oFormActMain.LastKnownTime.Subtract(new TimeSpan(0, 0, 30))
                            ).Sum
                            (
                                x => x.Damage.Number
                            ) / (Data.Duration.TotalSeconds < 30.0 ? Data.Duration.TotalSeconds : 30.0)
                        ).ToString("0.00")
                    ));

            if (!CombatantData.ExportVariables.ContainsKey("Last60DPS"))
                CombatantData.ExportVariables.Add("Last60DPS",
                    new CombatantData.TextExportFormatter(
                        "Last60DPS",
                        "Last 60 Seconds DPS",
                        "Average DPS for last 60 seconds.",
                        (Data, ExtraFormat) =>
                        (Data.Items[outD].Items["All"].Items.ToList().Where
                            (
                                x => x.Time >= ActGlobals.oFormActMain.LastKnownTime.Subtract(new TimeSpan(0, 0, 60))
                            ).Sum
                            (
                                x => x.Damage.Number
                            ) / (Data.Duration.TotalSeconds < 60.0 ? Data.Duration.TotalSeconds : 60.0)
                        ).ToString("0.00")
                    ));

            if (!CombatantData.ExportVariables.ContainsKey("Last180DPS"))
                CombatantData.ExportVariables.Add("Last180DPS",
                    new CombatantData.TextExportFormatter(
                        "Last180DPS",
                        "Last 180 Seconds DPS",
                        "Average DPS for last 180 seconds.",
                        (Data, ExtraFormat) =>
                        (Data.Items[outD].Items["All"].Items.ToList().Where
                            (
                                x => x.Time >= ActGlobals.oFormActMain.LastKnownTime.Subtract(new TimeSpan(0, 0, 180))
                            ).Sum
                            (
                                x => x.Damage.Number
                            ) / (Data.Duration.TotalSeconds < 180.0 ? Data.Duration.TotalSeconds : 180.0)
                        ).ToString("0.00")
                    ));

            if (!CombatantData.ExportVariables.ContainsKey("overHeal"))
            {
                CombatantData.ExportVariables.Add
                (
                    "overHeal",
                    new CombatantData.TextExportFormatter
                    (
                        "overHeal",
                        "Overheal",
                        "Amount of healing that made flood over 100% of health.",
                        (Data, ExtraFormat) =>
                        (
                            (
                                // Data.Items[outD].Items["All"].Items.ToList().Where
                                Data.Items[outH].Items.ToList().Where
                                (
                                    x => x.Key == "All"
                                ).Sum
                                (
                                    x => x.Value.Items.ToList().Where
                                    (
                                        y => y.Tags.ContainsKey("overheal")
                                    ).Sum
                                    (
                                        y => Convert.ToInt64(y.Tags["overheal"])
                                    )
                                )
                            ).ToString()
                        )
                    )
                );
            }

            if (!CombatantData.ExportVariables.ContainsKey("damageShield"))
            {
                CombatantData.ExportVariables.Add
                (
                    "damageShield",
                    new CombatantData.TextExportFormatter
                    (
                        "damageShield",
                        "Damage Shield",
                        "Damage blocked by Shield skills of healer.",
                        (Data, ExtraFormat) =>
                        (
                            (
                                Data.Items[outH].Items.ToList().Where
                                (
                                    x => x.Key == "All"
                                ).Sum
                                (
                                    x => x.Value.Items.Where
                                    (
                                        y =>
                                        {
                                            if (y.DamageType == "DamageShield")
                                                return true;
                                            else
                                                return false;
                                        }
                                    ).Sum
                                    (
                                        y => Convert.ToInt64(y.Damage)
                                    )
                                )
                            ).ToString()
                        )
                    )
                );
            }

            if (!CombatantData.ExportVariables.ContainsKey("absorbHeal"))
            {
                CombatantData.ExportVariables.Add
                (
                    "absorbHeal",
                    new CombatantData.TextExportFormatter
                    (
                        "absorbHeal",
                        "Healed by Absorbing",
                        "Amount of heal, done by absorbing.",
                        (Data, ExtraFormat) =>
                        (
                            (
                                Data.Items[outH].Items.ToList().Where
                                (
                                    x => x.Key == "All"
                                ).Sum
                                (
                                    x => x.Value.Items.Where
                                    (
                                        y => y.DamageType == "Absorb"
                                    ).Sum
                                    (
                                        y => Convert.ToInt64(y.Damage)
                                    )
                                )
                            ).ToString()
                        )
                    )
                );
            }

            if (!EncounterData.ExportVariables.ContainsKey("CurrentRealUserName"))
            {
                EncounterData.ExportVariables.Add("CurrentRealUserName",
                new EncounterData.TextExportFormatter("CurrentRealUserName", "Current Real Username", "Current Username Not Fixed < YOU >", (Data, Extra, Format) => { return getCurrentPlayerName(); }));
            }

            if (!EncounterData.ExportVariables.ContainsKey("CurrentZoneRaw"))
            {
                EncounterData.ExportVariables.Add("CurrentZoneRaw",
                new EncounterData.TextExportFormatter("CurrentZoneRaw", "Current Zone ID", "Current Zone Real ID", (Data, Extra, Format) => { return getCurrentZone().ToString("X"); }));
            }

            ActGlobals.oFormActMain.ValidateLists();
        }

        public static void ParseData(bool isImport, LogLineEventArgs e)
        {
            string[] data = e.logLine.Split('|');
            if (data.Length < 2 || data == null) return;
            switch ((MessageType)Convert.ToInt32(data[0]))
            {
                case MessageType.ChangePrimaryPlayer:
                    if (data.Length < 4) return;

                    currentPlayerName = data[3];
                    break;
                case MessageType.ChangeZone:
                    if (data.Length < 3) return;

                    currentZone = Convert.ToInt32(data[2], 16);
                    break;
            }
        }

        public static string currentPlayerName = "YOU";
        public static string getCurrentPlayerName()
        {
            return currentPlayerName;
        }

        public static int currentZone = 0;
        public static int getCurrentZone()
        {
            return currentZone;
        }

        public enum MessageType
        {
            LogLine = 0,
            ChangeZone = 1,
            ChangePrimaryPlayer = 2,
            AddCombatant = 3,
            RemoveCombatant = 4,
            AddBuff = 5,
            RemoveBuff = 6,
            FlyingText = 7,
            OutgoingAbility = 8,
            IncomingAbility = 10,
            PartyList = 11,
            PlayerStats = 12,
            CombatantHP = 13,
            NetworkStartsCasting = 20,
            NetworkAbility = 21,
            NetworkAOEAbility = 22,
            NetworkCancelAbility = 23,
            NetworkDoT = 24,
            NetworkDeath = 25,
            NetworkBuff = 26,
            NetworkTargetIcon = 27,
            NetworkRaidMarker = 28,
            NetworkTargetMarker = 29,
            NetworkBuffRemove = 30,
            Debug = 251,
            PacketDump = 252,
            Version = 253,
            Error = 254,
            Timer = 255,
        }
    }
}
