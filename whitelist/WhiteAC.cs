  using Rocket.API;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace walterhcain.whitelistAll
{
    public class WhiteAC : IRocketPluginConfiguration
    {
        public CSteamID emsGroup;
        public CSteamID policeGroup;

        public void LoadDefaults()
        {
            emsGroup = (CSteamID)103582791455798051;
            policeGroup = (CSteamID)103582791461613862;
        }
    }
}
