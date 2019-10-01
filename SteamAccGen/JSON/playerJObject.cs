using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamAccGen.JSON
{
    class playerJObject
    {
        public string SteamID { get; set; }
        public string CommunityBanned { get; set; }
        public string VACBanned { get; set; }
        public string VACBans { get; set; }
        public string DaysSinceBan { get; set; }
        public string GameBans { get; set; }
        public string EconomyBan { get; set; }
    }
}
