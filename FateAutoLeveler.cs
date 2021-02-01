using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using ff14bot;
using ff14bot.Behavior;
using ff14bot.Helpers;
using ff14bot.Interfaces;
using ff14bot.Managers;
using ff14bot.RemoteWindows;
using Newtonsoft.Json;
using Buddy.Coroutines;
using TreeSharp;

namespace FateAutoLeveler
{
    public class FateAutoLeveler : IBotPlugin
    {
        #region Basics
        public string Author { get { return "Sidgurdan"; } }
        public Version Version { get { return new Version(1, 3); } }
        public string Name { get { return "Fate Auto Leveler"; } }
        public string EnglishName { get { return "Fate Auto Leveler"; } }
        public string Description { get { return "Moves your character to appropriate level range."; } }
        public bool WantButton
        {
            get { return false; }
        }
        public string ButtonText
        {
			       get { return "FateAutoLeveler"; }
        }
        public void OnButtonPress() {}
        public bool Equals(IBotPlugin other)
        {
            throw new NotImplementedException();
        }
        #endregion
        
        public void OnPulse()
        {
        
        }

        private Composite _coroutine;
        public void OnInitialize()
        {
            _coroutine = new ActionRunCoroutine(r => ChangeZone());
        }

        public void OnShutdown()
        {

        }
        
        public void OnEnabled()
        {
            Log("Enabled");
            TreeHooks.Instance.AddHook("TreeStart", _coroutine);
            TreeHooks.Instance.OnHooksCleared += OnHooksCleared;
        }
        
        public void OnDisabled()
        {
            Log("Disabled");
            TreeHooks.Instance.OnHooksCleared -= OnHooksCleared;
            TreeHooks.Instance.RemoveHook("TreeStart", _coroutine);
        }

        private void OnHooksCleared(object sender, EventArgs args)
        {
            TreeHooks.Instance.AddHook("TreeStart", _coroutine);
        }

        #region Change Zone Logic
        internal async Task<bool> ChangeZone()
        {
            // Leave if in wrong BotBase
            if (BotManager.Current.Name != "Fate Bot" && BotManager.Current.Name != "ExFateBot")
            {
                return false;
            }

            string nextLocation = NextLocation(); 

            // Leave if there is no next location
            if (nextLocation == AetheriteName.None)
            {
                Logging.Write("There is not recommended zone for your level registered.");
                return false;
            }
            
            // Leave if player is already on the recommended map
            if (CurrentLocation(WorldManager.ZoneId) == nextLocation)
            {
                return false;
            }

            if (Core.Player.IsMounted)
            {
                await CommonTasks.StopAndDismount();
            }

            UInt32 aetheriteId = FindAetheriteId(nextLocation);

            if (aetheriteId == 0)
            {
                Log("The required aetherite is not yet known by your character. You need: " + nextLocation);
                
                return false;
            }

            Log("Teleporting to " + nextLocation);

            await CommonTasks.Teleport(aetheriteId);

            while (NowLoading.IsVisible)
            {
                Log("Loading Screen detected. Waiting for 3s!");
                await Coroutine.Sleep(3000);
            }

            return true;
        }


        public string NextLocation()
        {
            string zone = AetheriteName.None;

			var currentLevel = (int)Core.Player.ClassLevel;

			if (currentLevel >= 1 && currentLevel < 10)
            {
                zone = AetheriteName.SummerfordFarms;
            }
            if (currentLevel >= 10 && currentLevel < 20)
            {
                zone = AetheriteName.Aleport;
            }
            if (currentLevel >= 20 && currentLevel < 25)
            {
                zone = AetheriteName.CampDrybone;
            }
            if (currentLevel >= 25 && currentLevel < 30)
            {
                zone = AetheriteName.Quarrymill;
            }
            if (currentLevel >= 30 && currentLevel < 35)
            {
                zone = AetheriteName.CostaDelSol;
            }
            if (currentLevel >= 35 && currentLevel < 40)
            {
                zone = AetheriteName.CampDragonhead;
            }
            if (currentLevel >= 40 && currentLevel < 47)
            {
                zone = AetheriteName.CampDragonhead;
            }
            if (currentLevel >= 47 && currentLevel < 50)
            {
                zone = AetheriteName.CampOverlook;
            }
            if (currentLevel >= 70 && currentLevel < 73)
            {
                zone = AetheriteName.Stilltide;
            }
            if (currentLevel >= 73 && currentLevel < 74)
            {
                zone = AetheriteName.LydhaLran;
            }
            if (currentLevel >= 74 && currentLevel < 75)
            {
                // TODO Add Locations 75+ 
                zone = AetheriteName.Wolekdorf;
            }

			return zone;
        }

        private UInt32 FindAetheriteId(string teleportName)
        {
            foreach (var location in WorldManager.AvailableLocations)
            {
                if (location.Name == teleportName)
                {
                    return location.AetheryteId;
                }
            }

            return 0;
        }

        public string CurrentLocation(uint zoneid)
        {
            switch (zoneid)
            {
                case 152:
                    return AetheriteName.HawthorneHut;
                case 153:
                    return AetheriteName.Quarrymill;
                case 145:
                    return AetheriteName.CampDrybone;
                case 140:
                    return AetheriteName.Horizon;
                case 139:
                    return AetheriteName.CampBronzeLake;
                case 134:
                    return AetheriteName.SummerfordFarms;
                case 138:
                    return AetheriteName.Aleport;
                case 137:
					return AetheriteName.CostaDelSol;
                case 155:
					return AetheriteName.CampDragonhead;
                case 180:
                    return AetheriteName.CampOverlook;
                case 814:
                    return AetheriteName.Stilltide;
                case 813:
                    return AetheriteName.FortJobb;
                case 816:
                    return AetheriteName.LydhaLran;
                default:
                    LogZone();
                    return AetheriteName.None;
            }
        }

        #endregion

        #region Debugging
        static void Log(string message)
        {
            Logging.Write(Colors.SkyBlue, "[FateAutoLeveler] " + message);
        }

        static void LogZone()
        {
            Logging.Write(
                Colors.SkyBlue,
                "[FateAutoLeveler] Unknown Zone Id: {0} | Raw Zone Id: {1} | Subzone Id: {2}", 
                WorldManager.ZoneId, 
                WorldManager.RawZoneId, 
                WorldManager.SubZoneId
            );
        }
        #endregion
    }
 }
