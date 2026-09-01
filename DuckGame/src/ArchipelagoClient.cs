using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.ModelBinding;
using System.Windows.Forms;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.Models;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
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
        private static readonly Dictionary<string,int> medalOrder = new Dictionary<string, int>{
            {"Bronze",1},
            {"Silver",2},
            {"Gold",3},
            {"Platinum",4},
            {"Developer",5},
        };
        private static ArchipelagoSession session;
        private static DeathLinkService deathLink;
        public static int deathLinkAmnesty;
        public static string slot = "";
        public static string address = "archipelago.gg";
        public static string port = "38281";
        public static string pass = "";
        private static List<string> availableLevels = new List<string>();
        private static List<Type> availableItems = new List<Type>();
        private static List<PopupQueueData> popupQueue = new List<PopupQueueData>();
        private static bool firstPopupRun = true;
        private class PopupQueueData {
            public string text;
            public PopupQueueData(string itemName, bool received, ItemFlags iclass){
                if (received){
                    text += "Recv: ";
                }else{
                    text += "Sent: ";
                }
                if (iclass == ItemFlags.Advancement){text+="|PURPLE|";}
                else if (iclass == ItemFlags.NeverExclude){text+="|BLUE|";}
                else if (iclass == ItemFlags.None){text+="|AQUA|";}
                else if (iclass == ItemFlags.Trap){text+="|RED|";}
                text += itemName;
            }
            public PopupQueueData(string firstText, string newColour, string secondText){
                text = firstText + newColour + secondText;
            }
        }
        private static List<FillerQueueData> fillerQueue = new List<FillerQueueData>();
        private static bool firstFillerRun = true;
        private class FillerQueueData {
            public string itemName;
            public ItemFlags itemClass;
            public FillerQueueData(string newItemName, ItemFlags newItemClass){
                itemName = newItemName;
                itemClass = newItemClass;
            }
        }
        public static SlotData slotData = new SlotData();
        public class SlotData {
            public long medalCountGoal;
            public bool sendLowerMedals;
            public List<string> enabledMedals;
            private bool deathLinkEnabled;
            public bool DeathLinkEnabled{
                get { return deathLinkEnabled; }
                set {
                    deathLinkEnabled = value;
                    if (deathLink!=null){
                        if (deathLinkEnabled){
                            deathLink.EnableDeathLink();
                        }else{
                            deathLink.DisableDeathLink();
                        }}}
            }
            public int deathLinkAmnestyMax;
            public SlotData(long medalCount=150,bool lowerMedals=true,List<string>enabledMedalsNew=null,bool newdeathLinkEnabled=false,long newdeathLinkAmnesty=0){
                medalCountGoal = medalCount;
                sendLowerMedals = lowerMedals;
                enabledMedals = enabledMedalsNew;
                deathLinkEnabled = newdeathLinkEnabled;
                deathLinkAmnestyMax = (int)newdeathLinkAmnesty;
            }
        }
        private static ChallengeLevel currentLevel;
        private static Duck currentDuck;

        public static void Connect(){
            availableLevels.Clear();
            availableItems.Clear();
            popupQueue.Clear();
            firstPopupRun = true;
            fillerQueue.Clear();
            firstFillerRun = true;
            bool success = int.TryParse(port, out int portNum);
            if (!success){
                return;
            }
            session = ArchipelagoSessionFactory.CreateSession(address, portNum);
            session.Items.ItemReceived += ItemReceived;
            session.Socket.SocketClosed += SessionSocketClosed;
            session.Socket.ErrorReceived += SessionErrorReceived;

            LoginResult result;

            try{
                // handle TryConnectAndLogin attempt here and save the returned object to `result`
                result = session.TryConnectAndLogin("DuckGame", slot, ItemsHandlingFlags.AllItems,null,null,null,pass,true);
            }
            catch (Exception e){
                result = new LoginFailure(e.GetBaseException().Message);
            }
            if (!result.Successful){
                HUD.AddInputChangeDisplay("@UNPLUG@|RED|Could not connect to AP");
                LoginFailure failure = (LoginFailure)result;
                string errorMessage = $"Failed to Connect to {address} as {slot}:";
                foreach (string error in failure.Errors){
                    errorMessage += $"\n    {error}";
                }
                foreach (ConnectionRefusedError error in failure.ErrorCodes){
                    errorMessage += $"\n    {error}";
                }

                return; // Did not connect, show the user the contents of `errorMessage`
            }
            LoginSuccessful loginSuccess = (LoginSuccessful)result;
            deathLink = DeathLinkProvider.CreateDeathLinkService(session);
            deathLink.OnDeathLinkReceived += DeathLinkReceived;
            deathLinkAmnesty = 0;
            List<string> enabledMedals = new List<string>(){}; 
            foreach(string medal in medalOrder.Keys){
                if ((long)loginSuccess.SlotData["use_"+medal.ToLower()+"_medal"]!=0){
                    enabledMedals.Add(medal);
                }
            }
            slotData.medalCountGoal = (long)loginSuccess.SlotData["medal_count_goal"];
            slotData.sendLowerMedals = (long)loginSuccess.SlotData["send_lower_medals"]!=0;
            slotData.enabledMedals = enabledMedals;
            slotData.DeathLinkEnabled = (long)loginSuccess.SlotData["death_link"]!=0;
            slotData.deathLinkAmnestyMax = (int)(long)loginSuccess.SlotData["death_link_amnesty"];
            HUD.AddInputChangeDisplay("@PLUG@|LIME|AP Connected");
            CheckIfGoaled();
        }
        public static void Disconnect(){
            if (session!=null&&session.Socket.Connected){
                session.Socket.DisconnectAsync().ContinueWith(t => (session=null,deathLink=null));
            }else{session = null;deathLink=null;}
            availableLevels.Clear();
            availableItems.Clear();
            popupQueue.Clear();
            fillerQueue.Clear();
            HUD.AddInputChangeDisplay("@UNPLUG@|RED|AP Disconnected");
        }
        public static bool CheckConnection(){
            if (session!=null){
                if (!session.Socket.Connected){
                    HUD.AddInputChangeDisplay("@UNPLUG@|RED|AP Disconnected");
                    return false;
                }
                return true;
            }
            HUD.AddInputChangeDisplay("@UNPLUG@|RED|AP Disconnected");
            return false;
        }
        public static bool CheckConnectionNoPopup(){
            if (session!=null){
                if (!session.Socket.Connected){
                    return false;
                }
                return true;
            }
            return false;
        }
        private static void ItemReceived(IReceivedItemsHelper helper){
            ItemInfo newItem = helper.DequeueItem();
            string itemName = newItem.ItemName;
            if (itemName != "Filler"){
                if (name2level.Keys.Contains(itemName)&&!availableLevels.Contains(name2level[itemName])){
                    availableLevels.Add(name2level[itemName]);
                }else if (name2item.Keys.Contains(itemName)&&!availableItems.Contains(name2item[itemName])){
                    availableItems.Add(name2item[itemName]);
                }else if (newItem.Flags == ItemFlags.Trap || newItem.Flags == ItemFlags.None){
                    fillerQueue.Add(new FillerQueueData(itemName,newItem.Flags));
                    return;
                }
                popupQueue.Add(new PopupQueueData(itemName,true,newItem.Flags));
            }
        }
        private static void SessionSocketClosed(string reason){
            Disconnect();
        }
        private static void SessionErrorReceived(Exception e, string message){
            Disconnect();
        }
        private static void DeathLinkReceived(DeathLink deathLink){
            popupQueue.Insert(0, new PopupQueueData("DL: |AQUA|"+deathLink.Source+" |WHITE|- ","|RED|",deathLink.Cause));
            if (Level.current==currentLevel){
                currentDuck.Kill(new DTCrush(currentDuck),false);
            }else if (Level.current is ArcadeLevel){
                (Level.current as ArcadeLevel).RagdollDuck();
            }
        }
        public static void SendDeathLink(string cause){
            if (session==null||deathLink==null){return;}
            if (slotData.DeathLinkEnabled){
                if (slotData.deathLinkAmnestyMax!=0){
                    deathLinkAmnesty+=1;
                    if (deathLinkAmnesty>=slotData.deathLinkAmnestyMax){
                        deathLinkAmnesty=0;
                        popupQueue.Insert(0, new PopupQueueData("SEND DL: ","|RED|",cause));
                        deathLink.SendDeathLink(new DeathLink(slot,cause));
                    }}}
        }
        public static bool LevelExists(string level){
            CheckConnection();
            if (level2name.Keys.Contains(level)){return availableLevels.Contains(level);}
            return true;
        }
        public static bool ItemExists(Thing item){
            CheckConnection();
            if (name2item.Values.Contains(item.GetType())){return availableItems.Contains(item.GetType());}
            return true;
        }
        public static string GetAllItems(){
            CheckConnection();
            string allItems="Items:\n";
            foreach (Type item in availableItems){
                allItems+=item.ToString().Substring(9)+"\n";
            }
            return allItems;
        }
        public static TrophyType GetBestTrophy(string level){
            TrophyType highestMedal = TrophyType.Baseline;
            if (session==null){return highestMedal;}
            foreach(string medal in slotData.enabledMedals){
                if (session.Locations.AllLocationsChecked.Contains(session.Locations.GetLocationIdFromName("DuckGame",level2name[level]+" "+medal+" Medal"))){
                    highestMedal=(TrophyType)medalOrder[medal];
                }else{
                    return highestMedal;
                }
            }
            return highestMedal;
        }
        private static bool CheckIfGoaled(int extra=0){
            if (session.Locations.AllLocationsChecked.Count+extra >= slotData.medalCountGoal){
                session.SetGoalAchieved();
                popupQueue.Clear();
                HUD.AddInputChangeDisplay("|LIME| GAME IS BEATEN YAYAY <333 ");
                return true;
            }
            return false;
        }
        public static void SendItem(string level,string wonMedal){
            if (session==null){return;}
            List<long> locations = new List<long>();
            if (slotData.sendLowerMedals){
                foreach (string medal in medalOrder.Keys.ToList().GetRange(0,medalOrder[wonMedal])){
                    if (slotData.enabledMedals.Contains(medal)){
                        long loc = session.Locations.GetLocationIdFromName("DuckGame",level2name[level]+" "+medal+" Medal");
                        if (loc == -1 || session.Locations.AllLocationsChecked.Contains(loc)){continue;}
                        locations.Add(loc);
                    }
                }
            }else{
                if (slotData.enabledMedals.Contains(wonMedal)){
                    long loc = session.Locations.GetLocationIdFromName("DuckGame",level2name[level]+" "+wonMedal+" Medal");
                    if (loc == -1 || session.Locations.AllLocationsChecked.Contains(loc)){return;}
                    locations.Add(loc);
                }
            }
            if (locations.Count > 0){
                if (CheckIfGoaled(locations.Count)){return;
                }else{
                    new Task(() => {session.Locations.ScoutLocationsAsync(locations.ToArray()).ContinueWith(t => DisplaySentItem(t.Result));}).Start();
                    session.Locations.CompleteLocationChecksAsync(locations.ToArray());
                }
            }
        }
        private static void DisplaySentItem(Dictionary<long, ScoutedItemInfo> packet){
            foreach(ScoutedItemInfo item in packet.Values){
                popupQueue.Add(new PopupQueueData(item.ItemName,false,item.Flags));
            }
        }
        public static void SetLevel(ChallengeLevel newLevel,Duck newDuck){
            currentLevel = newLevel;
            currentDuck = newDuck;
        }
        public static void ResetFillerFrames(){
            fillerFrame = 0;
            perFillerFrame = 0;
            perFillerTimes = 0;
        }
        public static void Update(){
            ProcessPopupQueue();
            if (CheckConnectionNoPopup()){
                ProcessFillerQueue();
            }
        }
        private static long popupFrame = 0;
        private static void ProcessPopupQueue(){
            if (popupFrame>0){
                popupFrame++;
                if (popupFrame == 75){
                    popupFrame = 0;
                }else{
                    return;
                }
            }
            if (firstPopupRun){
                firstPopupRun = false;
                popupQueue.Clear();
            }
            if (popupQueue.Count > 0){
                PopupQueueData item = popupQueue[0];
                popupQueue.RemoveAt(0);
                HUD.AddInputChangeDisplay(" "+item.text+" ");
                popupFrame++;
            }
        }
        private static long fillerFrame = 0;
        private static long perFillerFrame = 0;
        private static long perFillerTimes = 0;
        private static void ProcessFillerQueue(){
            if (fillerFrame>0){
                if (Level.current == currentLevel){
                    fillerFrame++;
                    if (fillerFrame == 240){
                        popupQueue.Insert(0, new PopupQueueData("Trap Complete","",""));
                        fillerQueue.RemoveAt(0);
                    }else if (fillerFrame == 300){
                        fillerFrame = 0;
                        perFillerFrame = 0;
                        perFillerTimes = 0;
                    }else{
                        return;
                    }
                }else{
                    return;
                }
            }
            if (firstFillerRun){
                firstFillerRun = false;
                fillerQueue.Clear();
            }
            if (fillerQueue.Count>0){
                if (Level.current == currentLevel){
                    FillerQueueData item = fillerQueue[0];
                    if (fillerFrame==0&&perFillerFrame==0&&perFillerTimes==0){
                        if (popupQueue.Count>0&&popupQueue[0].text=="Trap Complete"){popupQueue.Insert(1, new PopupQueueData(item.itemName,true,item.itemClass));}
                        else{popupQueue.Insert(0, new PopupQueueData(item.itemName,true,item.itemClass));}
                    }
                    switch(item.itemName){
                    case "Clumsy":
                        if (perFillerFrame==0&&perFillerTimes==0){
                            currentDuck.GoRagdoll();}
                        if (perFillerFrame == 180){
                            currentDuck.GoRagdoll();
                            perFillerFrame = 0;
                            perFillerTimes++;
                            if (perFillerTimes == 4){
                                fillerFrame++;
                            }
                        }
                        break;
                    case "Stop Hitting Yourself":
                        if (perFillerFrame==0&&perFillerTimes==0){
                            currentDuck.GiveBrainRot();}
                        if (perFillerFrame == 900){
                            currentDuck.RemoveBrainRot();
                            fillerFrame++;
                        }
                        break;
                    case "FIREEE!":
                        currentDuck.Burn(new Vec2(0,0),currentDuck);
                        fillerFrame=150;
                        break;
                    case "FIREEE*":
                        if (perFillerFrame==0&&perFillerTimes==0){
                            currentDuck.Burn(new Vec2(0,0),currentDuck);}
                        if (perFillerFrame == 190){
                            currentDuck.Extinquish();
                            fillerFrame++;
                        }
                        break;
                    case "Slippery Hands":
                        currentDuck.ThrowItem();
                        fillerFrame++;
                        break;
                    case "Whats Under There?":
                        if (perFillerFrame==0&&perFillerTimes==0){
                            currentDuck.AdvanceServerTime(5);}
                        if (perFillerFrame == 15){
                            currentDuck.AdvanceServerTime(5);
                            perFillerFrame = 0;
                            perFillerTimes++;
                            if (perFillerTimes == 24){
                                fillerFrame++;
                            }
                        }
                        break;
                    case "Caught You!":
                        currentDuck.Netted(new Net(0,0,currentDuck));
                        fillerFrame++;
                        break;
                    case "Duck Season":
                        if (perFillerFrame==0&&perFillerTimes==0){
                            SuicidePistol suicidePistol = new SuicidePistol(currentDuck.x,currentDuck.y);
                            Level.Add(suicidePistol,true);
                            currentDuck.GiveHoldable(suicidePistol);
                        }
                        fillerFrame++;
                        break;
                    case "Hot Potato":
                        Grenade grenade = new Grenade(0,0);
                        grenade.OnPressAction();
                        Level.Add(grenade,true);
                        currentDuck.GiveHoldable(grenade);
                        fillerFrame++;
                        break;
                    case "Whats This?":
                        Holdable randomItem = ItemBoxRandom.GetRandomItem<Holdable>();
                        randomItem.position = new Vec2(0,0);
                        Level.Add(randomItem,true);
                        currentDuck.GiveHoldable(randomItem);
                        fillerFrame++;
                        break;
                    case "esreveR":
                        if (perFillerFrame==0&&perFillerTimes==0){
                            currentDuck.moveReverse=-1;}
                        if (perFillerFrame == 600){
                            currentDuck.moveReverse=1;
                            fillerFrame++;
                        }
                        break;
                    case "A Gift for you":
                        DeathCrate crate = new DeathCrate(currentDuck.position.x,currentDuck.position.y);
                        crate.settingIndex = (byte)Rando.Int(12);
                        Level.Add(crate,true);
                        crate.activated = true;
                        fillerFrame++;
                        break;
                    case "Don't look up":
                        long amount = 5;
                        float seperation = (currentLevel.topLeft.x-currentLevel.bottomRight.x)/amount;
                        for (int i = 0; i < amount; i++)
                        {
                            for (int a = 0; a < amount; a++)
                            {
                                Grenade gren1 = new Grenade(seperation*i+currentDuck.position.x,currentDuck.position.y+a*seperation);
                                gren1.OnPressAction();
                                Level.Add(gren1,true);
                                Grenade gren2 = new Grenade(-seperation*i+currentDuck.position.x,currentDuck.position.y-a*seperation);
                                gren2.OnPressAction();
                                Level.Add(gren2,true);
                                Grenade gren3 = new Grenade(seperation*i+currentDuck.position.x,currentDuck.position.y+a*-seperation);
                                gren3.OnPressAction();
                                Level.Add(gren3,true);
                                Grenade gren4 = new Grenade(-seperation*i+currentDuck.position.x,currentDuck.position.y-a*-seperation);
                                gren4.OnPressAction();
                                Level.Add(gren4,true);
                            }
                        }
                        fillerFrame++;
                        break;
                    case "Prot V":
                        Helmet newHelmet = new Helmet(0,0);
                        Level.Add(newHelmet,true);
                        currentDuck.Equip(newHelmet);
                        ChestPlate newChestPlate = new ChestPlate(0,0);
                        Level.Add(newChestPlate,true);
                        currentDuck.Equip(newChestPlate);
                        Boots newBoots = new Boots(0,0);
                        Level.Add(newBoots,true);
                        currentDuck.Equip(newBoots);
                        fillerFrame++;
                        break;
                    case "Make it rain":
                        if (currentDuck.holdObject is Gun){
                            Gun ob = currentDuck.holdObject as Gun;
                            ob.ammo = 999999;
                        }
                        fillerFrame++;
                        break;
                    default:
                        fillerQueue.RemoveAt(0);
                        return;
                    }
                    perFillerFrame++;
                }
            }
            // MODIFIERS
            // QWOP mode
            // DevConsole.qwopMode = true;
        }
    }
}
