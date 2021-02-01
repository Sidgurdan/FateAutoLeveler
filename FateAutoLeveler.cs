using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Threading;
using ff14bot;
using ff14bot.Helpers;
using ff14bot.Interfaces;
using ff14bot.Managers;
using Newtonsoft.Json;

namespace FateAutoLeveler
{
    public class FateAutoLeveler : IBotPlugin
    {
        #region Necessary Stuff
        public string Author { get { return "Zamphire"; } }//version by rysertom
        public Version Version { get { return new Version(1, 1); } }
        public string Name { get { return "Fate Auto Leveler"; } }
        public string Description { get { return "Moves your character to appropriate level range."; } }
        public bool WantButton
        {
            get { return false; }
        }
        public string ButtonText
        {
			get { return "FateAutoLeveler"; }
        }
        public void OnButtonPress()
        {
 
        }
        public bool Equals(IBotPlugin other)
        {
            throw new NotImplementedException();
        }
        #endregion
        public void OnPulse()
        {
            if (BotManager.Current.Name == "Fate Bot")
            {
                ChangeZone();
            }
        }

        public void OnInitialize()
        {

        }
        public void OnShutdown()
        {

        }
        public void OnEnabled()
        {

        }
        public void OnDisabled()
        {

        }

        #region Atmas
        public void ChangeZone()
        {
            #region Change zones
                if (NextLocation() == "None")
                {
                    Logging.Write("You don't have the Aetheryte for this level range.");
                    return;
                }

                if (CurrentLocation(WorldManager.ZoneId) == NextLocation())
                {
                    return;
                }

                //TreeRoot.Stop();

                if (Core.Player.IsMounted)
                {
                    ActionManager.Dismount();
                    Thread.Sleep(1000);
                }

                if (CurrentLocation(WorldManager.ZoneId) != NextLocation())
                {
                    Logging.Write("[FateBot] Moving to " + NextLocation());
                    if (NextLocation() == "Dravanian Hinterlands")
                        WorldManager.TeleportById(76);
                    else
                        WorldManager.TeleportById(Aetheryteid());
                    Thread.Sleep(15000);
                    //TreeRoot.Start();

                }
            

            #endregion
        }

		
        public string NextLocation()
        {
            string zone = null;
			
			var currentLevel = (int)Core.Player.ClassLevel;
			
			if (currentLevel >= 10 && currentLevel < 20)
            {
                zone = "10thru20";
            }
			if (currentLevel >= 20 && currentLevel <30)
            {
                zone = "20thru30";
            }
			if (currentLevel >= 30 && currentLevel < 35)
            {
                zone = "30thru35";
            }
			if (currentLevel >= 35 && currentLevel < 45)
            {
                zone = "35thru45";
            }
			if (currentLevel >= 45 && currentLevel < 50)
            {
                zone = "45thru50";
            }
			if (currentLevel >= 50 && currentLevel < 53)
            {
                zone = "50thru53";
            }
			if (currentLevel >= 53 && currentLevel < 57)
            {
                zone = "53thru57";
            }
			if (currentLevel >= 57 && currentLevel < 60)
            {
                zone = "57thru60";
            }
            
//
//            foreach (BagSlot a in InventoryManager.Slots)
  //          {
    //            Inventory.Add(a.Name);
      //      }
//
  //          if (!Inventory.Contains(jstAtma))
    //        {
      //          return GetLocationName(jstAtma);
        //    }
//
  //          foreach (string a in Atmas)
    //        {
      //          if (!Inventory.Contains(a))
        //            atma = a;
          //  }
//
  //          return GetLocationName(zone);
			return GetLocationName(zone);
        }

        private static string GetLocationName(string zone)
        {
            switch (zone)
            {
                case "10thru20":
                    return "Aleport";//138;

                case "20thru30":
                    return "Quarrymill";//153;

                case "30thru35":
                    return "Costa del Sol";//137;

                case "35thru45":
                    return "Camp Dragonhead";//155;

                case "45thru50":
                    return "Ceruleum Processing Plant";//147;

                case "50thru53":
                    return "Coerthas Western Highlands";//397;

                case "53thru57":
                    return "Churning Mists";//400;
					
		case "57thru60":
                    return "Dravanian Hinterlands";//398;

                default:
                    return "None";
            }
        }
        public string CurrentLocation(uint zoneid)
        {
            switch (zoneid)
            {
                case 138:
                	return "Aleport";//138;
					
		case 153:
			return "Quarrymill";//153;
					
                case 137:
                	return "Costa del Sol";//137;

                case 155:
                    	return "Camp Dragonhead";//155;

                case 147:
                    	return "Ceruleum Processing Plant";//147;

                case 397:
                    	return "Coerthas Western Highlands";//397;

                case 400:
                    	return "Churning Mists";//400;
					
		case 398:
			return "Dravanian Hinterlands";//398;

                default:
                    return "None";
            }
        }
		public uint Aetheryteid()
		{
			switch(NextLocation())
			{
				case "Aleport":
					return 14;
					
				case "Quarrymill":
					return 5;
					
				case "Costa del Sol":
					return 11;
					
				case "Camp Dragonhead":
					return 23;
					
				case "Ceruleum Processing Plant":
					return 22;
					
				case "Coerthas Western Highlands":
					return 71;
					
				case "Churning Mists":
					return 78;
					
				case "Dravanian Hinterlands":
					return 78;

                		default:
                    			return 2;
			}
		}

        #endregion
    }
 }

