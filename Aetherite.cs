using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ff14bot.Managers;

namespace FateAutoLeveler
{
    public class Aetherite
    {
        public UInt32 ZoneId;
        public string Name;
        public uint MinLevel;
        public uint MaxLevel;

        public Aetherite(UInt32 locationZoneId, string locationName, uint min, uint max)
        {
            ZoneId = locationZoneId;
            Name = locationName;
            MinLevel = min;
            MaxLevel = max;
        }
    }
}
