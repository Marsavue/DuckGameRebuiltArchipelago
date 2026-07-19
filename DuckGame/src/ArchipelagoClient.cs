using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.ModelBinding;
using System.Windows.Forms;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using HarmonyLib;

namespace DuckGame
{   
    internal static class ArchipelagoClient
    {
        private static readonly Dictionary<string,string> name2level = new Dictionary<string, string>{
            { "VARIETY ZONE - OBSTACLE COURSE", "1abc0a39-09e1-424f-9e21-9602c41b7da9" },
            { "VARIETY ZONE - DEATH RAY 101", "3ece00ba-d342-42b3-b22f-4ed21d75d062" },
            { "VARIETY ZONE - SWING SHOES", "5a3ee55f-4149-4f2a-a222-95d1d52c8b8b" },
            { "TARGET MISSIONS - STEP 1: OFFICE RAID", "6e82639f-f0a1-4066-b08f-885392d81af7" },
            { "TARGET MISSIONS - STEP 2: INTEL", "a618f1b0-d21e-4799-a097-cceb9bbbd675" },
            { "TARGET MISSIONS - STEP 3: HEADQUARTERS", "ff770032-94a7-4ab3-a0ba-5b6592d0e6d4" },
            { "VARIETY ZONE 2 - MACE FACE", "588541bc-ffa3-4bdf-ac18-65c546818abe" },
            { "VARIETY ZONE 2 - PHASER 101", "8a35adf1-3d07-430f-9dff-85cc0d3499a7" },
            { "VARIETY ZONE 2 - SWING SHOTGUN", "8bfb3be0-6c48-40af-8cf9-d997580ad931" },
            { "SUPER SAW DUCK - WALL JUMP 101", "77ef8770-e7bf-44ac-b073-a7eadf8bf68d" },
            { "SUPER SAW DUCK - WOAH, SPIKES!", "cca244a5-946e-451c-9138-5ecf4776fdab" },
            { "SUPER SAW DUCK - SUPER DUCK CHAMP", "9c8c5dd0-b2c9-4897-a9a2-beac49d3b366" },
            { "CHAINSAW RACING - OPEN ROAD", "23ff1f65-cf6a-4065-8857-43b12a587bf8" },
            { "CHAINSAW RACING - PRO TOUR", "127e162f-d016-4ae1-8ae5-a51101675713" },
            { "CHAINSAW RACING - GRINDY 500", "8e5a2b40-4c7b-460b-89a0-ab7910c530ab" },
            { "OFFICE WORK - WORKING LATE", "a66c52a5-85fd-4399-b6c8-4a835c103771" },
            { "OFFICE WORK - DOOR CRASHER", "dc13e85a-ba4d-4819-afc9-b992b8a8fee1" },
            { "OFFICE WORK - INDUSTRIAL SHOOTOUT", "c5619b71-242f-48b9-8155-f430c66249fc" },
            { "WEAPON TRAINING - GRENADE LAUNCHER 101", "b63bda64-4e7a-48ce-9d35-a49847c6612c" },
            { "WEAPON TRAINING - MAGNUM TRAINING", "6a67885f-341f-4722-b91e-70074c26f713" },
            { "WEAPON TRAINING - CHAINGUN JETPACK", "9be5612f-e004-4e94-9aba-0831b58a0f22" },
            { "TELEPORTERS - TELE TWISTER", "882626cc-16a3-436c-84ce-19ed8c21369a" },
            { "TELEPORTERS - LABYRINTH", "f9761908-9d32-405e-adda-8ddf7c4a891d" },
            { "TELEPORTERS - DUCK DODGER", "c25b870c-3eef-41e6-a89f-0ef949c13ae4" },
            { "VARIETY ZONE FINAL - ASCENSION", "1b1188f7-6495-4317-a9dd-5f842d6bdec8" },
            { "VARIETY ZONE FINAL - SNIPER 101", "0ca36b08-c2bf-4dc7-bc19-492abf3691eb" },
            { "VARIETY ZONE FINAL - SWING MACE", "9b081557-8042-4d31-a5dd-304b831f435b" },
            { "VARIETY ZONE FINAL II - GUN JUMPER", "79586b9f-c989-4851-afce-d6c296db97f3" },
            { "VARIETY ZONE FINAL II - REBOUND 101", "2d250f85-25f4-43ec-ad14-5010ce25eee2" },
            { "VARIETY ZONE FINAL II - SAW CHAMPION", "1597667b-bd53-42be-9ba6-7eab66b13625" },
        };
        private static readonly Dictionary<string,string> level2name = new Dictionary<string, string>{
            { "1abc0a39-09e1-424f-9e21-9602c41b7da9", "VARIETY ZONE - OBSTACLE COURSE"},
            { "3ece00ba-d342-42b3-b22f-4ed21d75d062", "VARIETY ZONE - DEATH RAY 101"},
            { "5a3ee55f-4149-4f2a-a222-95d1d52c8b8b", "VARIETY ZONE - SWING SHOES"},
            { "6e82639f-f0a1-4066-b08f-885392d81af7", "TARGET MISSIONS - STEP 1: OFFICE RAID"},
            { "a618f1b0-d21e-4799-a097-cceb9bbbd675", "TARGET MISSIONS - STEP 2: INTEL"},
            { "ff770032-94a7-4ab3-a0ba-5b6592d0e6d4", "TARGET MISSIONS - STEP 3: HEADQUARTERS"},
            { "588541bc-ffa3-4bdf-ac18-65c546818abe", "VARIETY ZONE 2 - MACE FACE"},
            { "8a35adf1-3d07-430f-9dff-85cc0d3499a7", "VARIETY ZONE 2 - PHASER 101"},
            { "8bfb3be0-6c48-40af-8cf9-d997580ad931", "VARIETY ZONE 2 - SWING SHOTGUN"},
            { "77ef8770-e7bf-44ac-b073-a7eadf8bf68d", "SUPER SAW DUCK - WALL JUMP 101"},
            { "cca244a5-946e-451c-9138-5ecf4776fdab", "SUPER SAW DUCK - WOAH, SPIKES!"},
            { "9c8c5dd0-b2c9-4897-a9a2-beac49d3b366", "SUPER SAW DUCK - SUPER DUCK CHAMP"},
            { "23ff1f65-cf6a-4065-8857-43b12a587bf8", "CHAINSAW RACING - OPEN ROAD"},
            { "127e162f-d016-4ae1-8ae5-a51101675713", "CHAINSAW RACING - PRO TOUR"},
            { "8e5a2b40-4c7b-460b-89a0-ab7910c530ab", "CHAINSAW RACING - GRINDY 500"},
            { "a66c52a5-85fd-4399-b6c8-4a835c103771", "OFFICE WORK - WORKING LATE"},
            { "dc13e85a-ba4d-4819-afc9-b992b8a8fee1", "OFFICE WORK - DOOR CRASHER"},
            { "c5619b71-242f-48b9-8155-f430c66249fc", "OFFICE WORK - INDUSTRIAL SHOOTOUT"},
            { "b63bda64-4e7a-48ce-9d35-a49847c6612c", "WEAPON TRAINING - GRENADE LAUNCHER 101"},
            { "6a67885f-341f-4722-b91e-70074c26f713", "WEAPON TRAINING - MAGNUM TRAINING"},
            { "9be5612f-e004-4e94-9aba-0831b58a0f22", "WEAPON TRAINING - CHAINGUN JETPACK"},
            { "882626cc-16a3-436c-84ce-19ed8c21369a", "TELEPORTERS - TELE TWISTER"},
            { "f9761908-9d32-405e-adda-8ddf7c4a891d", "TELEPORTERS - LABYRINTH"},
            { "c25b870c-3eef-41e6-a89f-0ef949c13ae4", "TELEPORTERS - DUCK DODGER"},
            { "1b1188f7-6495-4317-a9dd-5f842d6bdec8", "VARIETY ZONE FINAL - ASCENSION"},
            { "0ca36b08-c2bf-4dc7-bc19-492abf3691eb", "VARIETY ZONE FINAL - SNIPER 101"},
            { "9b081557-8042-4d31-a5dd-304b831f435b", "VARIETY ZONE FINAL - SWING MACE"},
            { "79586b9f-c989-4851-afce-d6c296db97f3", "VARIETY ZONE FINAL II - GUN JUMPER"},
            { "2d250f85-25f4-43ec-ad14-5010ce25eee2", "VARIETY ZONE FINAL II - REBOUND 101"},
            { "1597667b-bd53-42be-9ba6-7eab66b13625", "VARIETY ZONE FINAL II - SAW CHAMPION"},
        };
        private static readonly Dictionary<string,Type> name2item = new Dictionary<string,Type>{
            {"Crates",typeof(Crate)},
            {"Huge Laser",typeof(HugeLaser)},
            {"Jetpack",typeof(Jetpack)},
            {"Grapple",typeof(Grapple)},
            {"Boots",typeof(Boots)},
            {"Pistol",typeof(Pistol)},
            {"Snubby Pistol",typeof(SnubbyPistol)},
            {"Desk",typeof(Desk)},
            {"Key",typeof(Key)},
            {"Shotgun",typeof(Shotgun)},
            {"Magnum",typeof(Magnum)},
            {"Combat Shotgun",typeof(CombatShotgun)},
            {"Chaingun",typeof(Chaingun)},
            {"Mace Collar",typeof(MaceCollar)},
            {"Weight Ball",typeof(WeightBall)},
            {"Phaser",typeof(Phaser)},
            {"Wall Boots",typeof(WallBoots)},
            {"Flower",typeof(Flower)},
            {"Chainsaw",typeof(Chainsaw)},
            {"Virtual Shotgun",typeof(VirtualShotgun)},
            {"Blue Barrel",typeof(BlueBarrel)},
            {"Grenade",typeof(Grenade)},
            {"Quad Laser",typeof(QuadLaser)},
            {"Sniper",typeof(Sniper)},
            {"Grenade Launcher",typeof(GrenadeLauncher)},
            {"Chest Plate",typeof(ChestPlate)},
            {"Helmet",typeof(Helmet)},
            {"Sword",typeof(Sword)},
            {"Mag Blaster",typeof(MagBlaster)},
            {"Laser Rifle",typeof(LaserRifle)},
            {"AK47",typeof(AK47)},
        };
        // private static readonly Dictionary<Type,string> item2name = new Dictionary<Type,string>{
        //     {typeof(Crate),"Crates"},
        //     {typeof(HugeLaser),"Huge Laser"},
        //     {typeof(Jetpack),"Jetpack"},
        //     {typeof(Grapple),"Grapple"},
        //     {typeof(Boots),"Boots"},
        // };
        private static ArchipelagoSession session;
        private static List<string> availableLevels = new List<string>();
        private static List<Type> availableItems = new List<Type>();
        public static string slot = "";
        public static string address = "archipelago.gg";
        public static string port = "38281";
        public static string pass = "";

        public static void Connect()
        {
            availableLevels.Clear();
            availableItems.Clear();
            bool success = int.TryParse(port, out int portNum);
            if (!success){
                return;
            }
            session = ArchipelagoSessionFactory.CreateSession(address, portNum);
            session.Items.ItemReceived += Items_ItemReceived;

            LoginResult result;

            try
            {
                // handle TryConnectAndLogin attempt here and save the returned object to `result`
                result = session.TryConnectAndLogin("DuckGame", slot, ItemsHandlingFlags.AllItems,null,null,null,pass,true);
            }
            catch (Exception e)
            {
                result = new LoginFailure(e.GetBaseException().Message);
            }

            if (!result.Successful)
            {
                LoginFailure failure = (LoginFailure)result;
                string errorMessage = $"Failed to Connect to {address} as {slot}:";
                foreach (string error in failure.Errors)
                {
                    errorMessage += $"\n    {error}";
                }
                foreach (ConnectionRefusedError error in failure.ErrorCodes)
                {
                    errorMessage += $"\n    {error}";
                }

                return; // Did not connect, show the user the contents of `errorMessage`
            }
            
            // Successfully connected, `ArchipelagoSession` (assume statically defined as `session` from now on) can now be
            // used to interact with the server and the returned `LoginSuccessful` contains some useful information about the
            // initial connection (e.g. a copy of the slot data as `loginSuccess.SlotData`)
            var loginSuccess = (LoginSuccessful)result;
            // Console.WriteLine(loginSuccess);
        }
        public static void Disconnect()
        {
            if (session.Socket.Connected){
                session.Socket.DisconnectAsync();
            }
        }
        private static void Items_ItemReceived(IReceivedItemsHelper helper)
        {
            var newItem = helper.DequeueItem();
            var item_name = newItem.ItemName;
            if (item_name != "Filler"){
                if (name2level.Keys.Contains(item_name)&&!availableLevels.Contains(name2level[item_name])){
                    availableLevels.Add(name2level[item_name]);
                }else if (name2item.Keys.Contains(item_name)&&!availableItems.Contains(name2item[item_name])){
                    availableItems.Add(name2item[item_name]);
                }
            }
        }
        public static bool LevelExists(string level){
            return availableLevels.Contains(level);
        }
        public static bool ItemExists(Thing item){
            return availableItems.Contains(item.GetType());
        }
        public static void SendItem(string level,string medal){
            var loc = session.Locations.GetLocationIdFromName("DuckGame",level2name[level]+" "+medal);
            if (session.Locations.AllLocationsChecked.Count >= name2level.Count*2){session.SetGoalAchieved();}
            if (loc == -1){return;}
            session.Locations.CompleteLocationChecksAsync(loc);
        }
    }
}
