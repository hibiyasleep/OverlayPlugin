using System;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Linq;
using Advanced_Combat_Tracker;
using FFXIV_ACT_Plugin.Common;

namespace RainbowMage.OverlayPlugin
{
    /* Taken from FFIXV_ACT_Plugin.Logfile. Copy&pasted to avoid issues if the FFXIV plugin ever changes this enum. */
    public enum LogMessageType
    {
        LogLine,
        ChangeZone,
        ChangePrimaryPlayer,
        AddCombatant,
        RemoveCombatant,
        AddBuff,
        RemoveBuff,
        FlyingText,
        OutgoingAbility,
        IncomingAbility = 10,
        PartyList,
        PlayerStats,
        CombatantHP,
        ParsedPartyMember,
        NetworkStartsCasting = 20,
        NetworkAbility,
        NetworkAOEAbility,
        NetworkCancelAbility,
        NetworkDoT,
        NetworkDeath,
        NetworkBuff,
        NetworkTargetIcon,
        NetworkTargetMarker = 29,
        NetworkBuffRemove,
        NetworkGauge,
        NetworkWorld,
        Network6D,
        NetworkNameToggle,
        NetworkTether,
        NetworkLimitBreak,
        NetworkEffectResult,
        NetworkStatusList,
        NetworkUpdateHp,
        Settings = 249,
        Process,
        Debug,
        PacketDump,
        Version,
        Error,
        Timer
    }

    class FFXIVRepository
    {
        private static IDataRepository repository;
        private static IDataSubscription subscription;

        private static IDataRepository GetRepository()
        {
            if (repository != null)
                return repository;

            var FFXIV = ActGlobals.oFormActMain.ActPlugins.FirstOrDefault(x => x.lblPluginTitle.Text == "FFXIV_ACT_Plugin.dll");
            if (FFXIV != null && FFXIV.pluginObj != null)
            {
                try
                {
                    repository = (IDataRepository)FFXIV.pluginObj.GetType().GetProperty("DataRepository").GetValue(FFXIV.pluginObj);
                }
                catch (Exception ex)
                {
                    Registry.Resolve<ILogger>().Log(LogLevel.Error, Resources.FFXIVDataRepositoryException, ex);
                }
            }

            return repository;
        }

        private static IDataSubscription GetSubscription()
        {
            if (subscription != null)
                return subscription;

            var FFXIV = ActGlobals.oFormActMain.ActPlugins.FirstOrDefault(x => x.lblPluginTitle.Text == "FFXIV_ACT_Plugin.dll");
            if (FFXIV != null && FFXIV.pluginObj != null)
            {
                try
                {
                    subscription = (IDataSubscription)FFXIV.pluginObj.GetType().GetProperty("DataSubscription").GetValue(FFXIV.pluginObj);
                }
                catch (Exception ex)
                {
                    Registry.Resolve<ILogger>().Log(LogLevel.Error, Resources.FFXIVDataSubscriptionException, ex);
                }
            }

            return subscription;
        }

        public static Process GetCurrentFFXIVProcess()
        {
            var repo = GetRepository();
            if (repo == null) return null;

            return repo.GetCurrentFFXIVProcess();
        }

        public static uint GetPlayerID()
        {
            var repo = GetRepository();
            if (repo == null) return 0;

            return repo.GetCurrentPlayerID();
        }

        public static string GetPlayerName()
        {
            var repo = GetRepository();
            if (repo == null) return null;

            var playerId = repo.GetCurrentPlayerID();

            var playerInfo = repo.GetCombatantList().FirstOrDefault(x => x.ID == playerId);
            if (playerInfo == null) return null;

            return playerInfo.Name;
        }

        public static ReadOnlyCollection<FFXIV_ACT_Plugin.Common.Models.Combatant> GetCombatants()
        {
            var repo = GetRepository();
            if (repo == null) return null;

            return repo.GetCombatantList();
        }

        public static Language GetLanguage()
        {
            var repo = GetRepository();
            if (repo == null)
                return Language.Unknown;
            return repo.GetSelectedLanguageID();
        }

        // LogLineDelegate(uint EventType, uint Seconds, string logline);
        public static void RegisterLogLineHandler(Action<uint, uint, string> handler)
        {
            var sub = GetSubscription();
            sub.LogLine += new LogLineDelegate(handler);
        }

        // NetworkReceivedDelegate(string connection, long epoch, byte[] message)
        public static void RegisterNetworkParser(Action<string, long, byte[]> handler)
        {
            var sub = GetSubscription();
            if (sub != null)
                sub.NetworkReceived += new NetworkReceivedDelegate(handler);
        }

        // PartyListChangedDelegate(ReadOnlyCollection<uint> partyList, int partySize)
        //
        // Details: partySize may differ from partyList.Count.
        // In non-cross world parties, players who are not in the same
        // zone count in the partySize but do not appear in the partyList.
        // In cross world parties, nobody will appear in the partyList.
        // Alliance data members show up in partyList but not in partySize.
        public static void RegisterPartyChangeDelegate(Action<ReadOnlyCollection<uint>, int> handler)
        {
            var sub = GetSubscription();
            if (sub != null)
                sub.PartyListChanged += new PartyListChangedDelegate(handler);
        }
    }
}
