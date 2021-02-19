using System;
using System.Threading.Tasks;
using ff14bot;
using ff14bot.Behavior;
using ff14bot.Helpers;
using ff14bot.Interfaces;
using ff14bot.Managers;
using ff14bot.RemoteWindows;
using Buddy.Coroutines;
using System.Windows.Media;
using TreeSharp;

namespace FateAutoLeveler
{
    public class FateAutoLeveler : IBotPlugin
    {
        #region Basics
        public string Author { get { return "Sidgurdan"; } }
        public Version Version { get { return new Version(1, 5); } }
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

        #region Prerequirements and Hooks
        private Composite _coroutine;
        private bool _enabled;
        private uint? _disabledForLevel;

        public void OnPulse()
        {
            if (!CheckBotBase())
            {
                _enabled = false;

                return;
            }

            if (_disabledForLevel.HasValue && (uint) Core.Player.ClassLevel == _disabledForLevel)
            {
                _enabled = false;
                
                return;
            }

            _enabled = true;
        }

        public void OnInitialize()
        {
            _coroutine = new ActionRunCoroutine(r => ChangeZone());
            _enabled = true;
            _disabledForLevel = null;
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
            TreeHooks.Instance.OnHooksCleared -= OnHooksCleared;
            TreeHooks.Instance.RemoveHook("TreeStart", _coroutine);
        }

        private void DisablePluginUntilLevelChange()
        {
            _enabled = false;
            _disabledForLevel = (uint) Core.Player.ClassLevel;
        }

        private void OnHooksCleared(object sender, EventArgs args)
        {
            TreeHooks.Instance.AddHook("TreeStart", _coroutine);
        }

        private bool CheckBotBase()
        {
            return (BotManager.Current.Name == "Fate Bot" || BotManager.Current.Name == "ExFateBot" || BotManager.Current.Name == "LizExFateBot");
        }
        #endregion

        #region Change Zone Logic
        internal async Task<bool> ChangeZone()
        {
            // Skip if OnPulse disabled the plugin functionality
            if(_enabled == false)
            {
                return false;
            }

            var nextAetherite = FindNextAetherite();

            if (nextAetherite == null)
            {
                Log("There is no recommended zone for your level registered.");
                DisablePluginUntilLevelChange();

                return false;
            }

            bool playerIsOnCorrectMap = WorldManager.ZoneId == nextAetherite.ZoneId;
            if (playerIsOnCorrectMap)
            {
                return false;
            }            
            
            try
            {
                WorldManager.TeleportLocation aetherite = FindAetheriteInAvailableLocations(nextAetherite.Name);

                await PrepareForTeleport();

                Log("Teleporting to " + aetherite.Name);
                await CommonTasks.Teleport(aetherite.AetheryteId);
            }
            catch (Exception exception)
            {
                Logging.WriteVerbose(exception.ToString());
                LogZone();
                Log("The required aetherite is not yet known by your character. You need: " + nextAetherite.Name);
                Log("Please disable/enable the plugin once you have the aetherite acquired.");

                // TODO Manually MoveTo if aetherite is not known

                DisablePluginUntilLevelChange();

                return false;
            }

            return true;
        }

        private async Task<bool> PrepareForTeleport()
        {
            if (Core.Player.IsMounted)
            {
                await CommonTasks.StopAndDismount();
            }

            while (NowLoading.IsVisible)
            {
                Log("Loading Screen detected. Waiting for 3s!");
                await Coroutine.Sleep(3000);
            }

            return true;
        }

        

        private Aetherite FindNextAetherite()
        {
            int level = (int) Core.Player.ClassLevel;

            foreach (var entry in AetheriteName.GetTargetAetherites())
            {
                var aetherite = entry.Value;
                if (level >= aetherite.MinLevel && level <= aetherite.MaxLevel)
                {
                    return aetherite;
                }
            }

            return null;
        }

        private WorldManager.TeleportLocation FindAetheriteInAvailableLocations(string teleportName)
        {
            foreach (var location in WorldManager.AvailableLocations)
            {
                if (location.Name == teleportName)
                {
                    return location;
                }
            }

            throw new Exception("Requested Aetherite not found in known locations.");
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
