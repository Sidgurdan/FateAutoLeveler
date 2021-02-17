using System.Collections.Generic;

namespace FateAutoLeveler
{
    public static class AetheriteName
    {
        public const string None = "None";

        #region 2.0 Zones
        public const string HawthorneHut = "The Hawthorne Hut";
        public const string Quarrymill = "Quarrymill";
        public const string CampDrybone = "Camp Drybone";
        public const string Horizon = "Horizon";
        public const string CampBronzeLake = "Camp Bronze Lake";
        public const string SummerfordFarms = "Summerford Farms";
        public const string Aleport = "Aleport";
        public const string CostaDelSol = "Costa del Sol";
        public const string CampDragonhead = "Camp Dragonhead";
        public const string CampOverlook = "Camp Overlook";
        public const string FalconsNest = "Falcon's Nest";
        #endregion

        #region Heavensward 50-60
        public const string CoerthasWesternHighlands = "Coerthas Western Highlands";
        public const string ChurningMists = "Churning Mists";
        public const string Tailfeather = "Tailfeather";
        #endregion

        #region Stormblood 60-70
        public const string ThePeaksNorth = "Ala Gannha";
        public const string TheRubySea = "Tamamizu";
        public const string Yanxia = "Namai";
        public const string TheAximSteppe = "Reunion";
        public const string TheLochs = "Porta Praetoria";
        #endregion

        #region Shadowbringers 70-80
        public const string Stilltide = "Stilltide";
        public const string FortJobb = "Fort Jobb";
        public const string LydhaLran = "Lydha Lran";
        public const string Slitherbough = "Slitherbough";
        public const string Twine = "Twine";
        public const string Tomra = "Tomra";
        #endregion

        public static Dictionary<uint, Aetherite> GetTargetAetherites()
        {
            Dictionary<uint, Aetherite> aetheriteList = new Dictionary<uint, Aetherite>();

            uint i = 0;
            aetheriteList.Add(++i, new Aetherite(134, SummerfordFarms, 1, 9));
            aetheriteList.Add(++i, new Aetherite(138, Aleport, 10, 19));
            aetheriteList.Add(++i, new Aetherite(145, CampDrybone, 20, 24));
            aetheriteList.Add(++i, new Aetherite(153, Quarrymill, 25, 29));
            aetheriteList.Add(++i, new Aetherite(137, CostaDelSol, 30, 34));
            aetheriteList.Add(++i, new Aetherite(155, CampDragonhead, 35, 46));
            aetheriteList.Add(++i, new Aetherite(180, CampOverlook, 47, 49));
            aetheriteList.Add(++i, new Aetherite(397, CoerthasWesternHighlands, 50, 52));
            aetheriteList.Add(++i, new Aetherite(400, ChurningMists, 53, 56));
            aetheriteList.Add(++i, new Aetherite(398, Tailfeather, 57, 59));
            aetheriteList.Add(++i, new Aetherite(620, ThePeaksNorth, 60, 62));
            aetheriteList.Add(++i, new Aetherite(613, TheRubySea, 63, 64));
            aetheriteList.Add(++i, new Aetherite(614, Yanxia, 65, 65));
            aetheriteList.Add(++i, new Aetherite(622, TheAximSteppe, 66, 67));
            aetheriteList.Add(++i, new Aetherite(621, TheLochs, 68, 69));
            aetheriteList.Add(++i, new Aetherite(814, Stilltide, 70, 72));
            aetheriteList.Add(++i, new Aetherite(816, LydhaLran, 73, 73));
            aetheriteList.Add(++i, new Aetherite(817, Slitherbough, 74, 75));
            aetheriteList.Add(++i, new Aetherite(815, Twine, 76, 77));
            aetheriteList.Add(++i, new Aetherite(814, Tomra, 78, 80));

            return aetheriteList;
        }
    }
}