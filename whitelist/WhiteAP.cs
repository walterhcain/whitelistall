using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using Steamworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace walterhcain.whitelistAll
{
    public class WhiteAP:RocketPlugin<WhiteAC>
    {
        public static WhiteAP Instance;
        private string Build = "1.1.3";

        string cd;
        string fileName = "whitelist.dat";
        string sourceFile;



        protected override void Load()
        {
            Instance = this;
            Logger.Log("Cain's Whitelist All plugin is initializing");
            Logger.Log("Checking for valid whitelist file");
            U.Events.OnPlayerConnected += CainConnect;

            string cd = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).FullName;
            string fileName = "whitelist.dat";
            string sourceFile = Path.Combine(cd, fileName);
            if (!File.Exists(sourceFile))
            {
                Logger.Log("No whitelist.dat file found. One will be made on use of whitelistall command or one can be made prior to command use.");
            }
            else
            {
                Logger.Log("whitelist.dat file found!");
                File.CreateText(sourceFile);
                Logger.Log("Whitelist.dat created.");
            }


            cd = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).FullName;
            sourceFile = Path.Combine(cd, fileName);


            Logger.Log("Initialization complete");
            Logger.Log("Cain's Whitelist All plugin has been successfully loaded");
            Logger.Log("Version: " + Build);
        }

       

        protected override void Unload()
        {
            U.Events.OnPlayerConnected -= CainConnect;
            Logger.Log("Cain's Whitelist All plugin has been successfully unloaded");
        }
        
        private void CainConnect(UnturnedPlayer player)
        {
            Logger.Log("0");
            StreamReader reader = new StreamReader(sourceFile);

            string line;
            ulong res;
            char[] whitespace = new char[0];
            Logger.Log("1");
            while ((line = reader.ReadLine()) != null)
            {
                
                string[] starr = line.Split(whitespace);
                string name = "";
                string[] narr = new string[starr.Length - 1];
                Logger.Log("2");
                if (player.SteamGroupID != Configuration.Instance.emsGroup && player.SteamGroupID != Configuration.Instance.policeGroup)
                {
                    Logger.Log("3");
                    Array.ConstrainedCopy(starr, 1, narr, 0, starr.Length - 1);
                    Logger.Log("4");
                }
                else
                {
                    Logger.Log("5");
                    Array.ConstrainedCopy(starr, 2, narr, 0, starr.Length - 2);
                    Logger.Log("6");
                }
                name = String.Join(" ", narr);
                //Check for SteamID and check if in the EMS or Police group.
                ulong.TryParse(starr[0], out res);
                CSteamID cid = (CSteamID)res;
                if (player.CSteamID == cid)
                {
                    Logger.Log("7");
                    if (player.CharacterName != name || player.DisplayName != name)
                    {
                        player.Kick("Public or Private name does not match whitelisted name.");
                        break;
                    }
                    else
                    {
                        Logger.Log(name + " joined the server");
                        break;
                    }
                }
            }
            //Should only get here naturally if they are not whitelisted.
            reader.Close();
        }
        

    }
}
