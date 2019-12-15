using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Advanced_Combat_Tracker;

namespace RainbowMage.OverlayPlugin
{
    class FFXIVExportVariables
    {
        static string outH = CombatantData.DamageTypeDataOutgoingHealing;

        public static void Init()
        {
            // TODO: Profile and optimize if necessary.

            // The code below was taken under MIT license from https://github.com/ZCube/ACTWebSocket/blob/master/ACTWebSocket.Core/Functions/OverlayACTWork.cs.
            // Copyright (c) 2016 ZCube

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
        }
    }
}
