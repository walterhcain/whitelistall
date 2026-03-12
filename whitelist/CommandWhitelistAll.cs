using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace walterhcain.whitelistAll
{
    public class CommandWhitelistAll : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "whitelistall";

        public string Help => "Whitelist all Steam IDs in whitelist file";

        public string Syntax => string.Empty;

        public List<string> Aliases => new List<string>() { "wa" };

        public List<string> Permissions => new List<string> { "wc.whitelistall" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length == 0)
            {
                string cd = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
                string fileName = "whitelist.dat";
                string sourceFile = Path.Combine(cd, fileName);
                if (!File.Exists(sourceFile))
                {
                    if (caller is UnturnedPlayer)
                    {
                        UnturnedPlayer player = (UnturnedPlayer)caller;
                        UnturnedChat.Say(player, "No whitelist document found. Creating whitelist.dat");
                        File.CreateText(sourceFile);
                        UnturnedChat.Say(player, "Whitelist.dat created. Enter Steam64 IDs on individual lines and run command again.");
                    }
                    else
                    {
                        Logger.Log("No whitelist document found. Creating whitelist.dat");
                        File.CreateText(sourceFile);
                        Logger.Log("Whitelist.dat created. Enter Steam64 IDs on individual lines and run command again.");
                        return;
                    }
                }
                else
                {
                    StreamReader reader = new StreamReader(sourceFile);
                    string line;
                    ulong res;
                    int i = 0;
                    //This is meant to be a whitespace
                    char[] whitespace = new char[0];
                    while ((line = reader.ReadLine()) != null)
                    {
                        //Add parse to retrieve the content Steam ID and name
                        string[] starr = line.Split(whitespace);
                        if (ulong.TryParse(starr[0], out res))
                        {
                            CSteamID cid = (CSteamID)res;
                            Logger.Log("Whitelisting Steam64: " + line);//, SteamName: " + UnturnedPlayer.FromCSteamID(cid).SteamName);
                            SteamWhitelist.whitelist(cid, "wc.wa", (CSteamID)76561198042425577);
                            i++;
                        }
                    }
                    reader.Close();
                    if (caller is UnturnedPlayer)
                    {
                        UnturnedPlayer player = (UnturnedPlayer)caller;
                        UnturnedChat.Say(player, i.ToString() + " players have been whitelisted");
                        Logger.Log(i.ToString() + " players have been whitelisted");
                    }
                    else
                    {
                        Logger.Log(i.ToString() + " players have been whitelisted");
                    }

                }
            }
            else
            {
                if(caller is UnturnedPlayer)
                {
                    UnturnedPlayer player = (UnturnedPlayer)caller;
                    UnturnedChat.Say(player, "Improper Parameters", UnityEngine.Color.red);
                }
                else
                {
                    Logger.Log("Improper Parameters");
                }
            }
        }
    }
}
