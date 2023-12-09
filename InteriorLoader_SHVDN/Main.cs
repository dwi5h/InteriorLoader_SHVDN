using System.Windows.Forms;
using GTA;
using GTA.UI;
using GTA.Math;
using GTA.Native;
using DieptidiUtility_SHVDN;
using NativeUI;

namespace InteriorLoader_SHVDN
{
    public class Main : Script
    {
        #region Constant Variable
        const string CONFIG_NAME = "InteriorLoader_SHVDN";
        int INTERIOR_ID = 0; // Get from Helper
        uint ROOM_ONE_KEY = 0;  // Get from Helper
        uint ROOM_GARAGE_KEY = 0;  // Get from Helper
        Vector3 HOUSE_POSITION = new Vector3(0, 0, 0);
        Vector3 POINT_POSITION = new Vector3(0, 0, 0);
        #endregion

        MenuPool menuPool;
        UIMenu uIBaseMenu;
        bool _toggleGarageDoor = false;

        public Main()
        {
            setUpConfig();

            Tick += Main_Tick; ;
            KeyUp += Main_KeyUp;

            uIBaseMenu = new UIMenu("Interior Loader", "");
            uIBaseMenu.MouseControlsEnabled = true;
            UIBaseMenu_ItemBuilder();

            uIBaseMenu.OnItemSelect += UIBaseMenu_OnItemSelect;

            menuPool = new MenuPool();
            menuPool.Add(uIBaseMenu);

            toggleGarageDoor();
        }

        private void UIBaseMenu_ItemBuilder()
        {
            uIBaseMenu.AddItem(new UIMenuItem("Enter Main Room"));
            uIBaseMenu.AddItem(new UIMenuItem("Enter Garage Room"));
            uIBaseMenu.AddItem(new UIMenuItem("Activating Interior"));
            uIBaseMenu.AddItem(new UIMenuItem("Refresh Vehicles On Garage"));
            uIBaseMenu.AddItem(new UIMenuItem("Set Teleport Point"));
            uIBaseMenu.AddItem(new UIMenuItem("Teleport to Point"));
            uIBaseMenu.AddItem(new UIMenuItem("Teleport to House"));
        }

        private void UIBaseMenu_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
        {
            switch (index)
            {
                case 0:
                    refreshRoomOne();
                    break;
                case 1:
                    refreshGarageRoom();
                    break;
                case 2:
                    activateInterior();
                    break;
                case 3:
                    refreshVehiclesOnGarage();
                    break;
                case 4:
                    setTeleportPoint();
                    break;
                case 5:
                    teleportToPoint();
                    break;
                case 6:
                    teleportToMlo();
                    break;
                default:
                    break;
            }
            uIBaseMenu.Visible = false;
        }

        private void Main_Tick(object sender, System.EventArgs e)
        {
            menuPool.ProcessMenus();
        }

        private void Main_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.E)
            {
                _toggleGarageDoor = !_toggleGarageDoor;
                toggleGarageDoor();
                refreshVehiclesOnGarage();
            }
            if (e.KeyCode == Keys.F1)
            {
                try
                {
                    uIBaseMenu.Visible = !uIBaseMenu.Visible;
                }
                catch (System.Exception ex)
                {
                    Notification.Show("error: " + ex.Message);
                }
            }
        }

        #region Others
        void toggleGarageDoor()
        {
            var garageHash = 2407266048; // Get from Codewalker
            var doorGaragePosition = new Vector3(880.5433f, -1575.084f, 31.75186f); // Get from Helper Entity Front Player
            var playerPed = Game.Player.Character;
            var distance = World.GetDistance(playerPed.Position, doorGaragePosition);
            if (distance <= 5f && _toggleGarageDoor)
            {
                Helper.ToggleEnableDoorOpen(garageHash, doorGaragePosition, true);
                Notification.Show("~y~Garage Door ~w~Opened");
            }
            else if (distance <= 25f)
            {
                Helper.ToggleEnableDoorOpen(garageHash, doorGaragePosition, false);
                Notification.Show("~y~Garage Door ~w~Closed");
            }
        }
        #endregion

        #region function menu
        void activateInterior()
        {
            Function.Call(Hash.SET_INTERIOR_ACTIVE, INTERIOR_ID, true);
            Function.Call(Hash.DISABLE_INTERIOR, INTERIOR_ID, false);
        }
        void refreshRoomOne()
        {
            Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, Game.Player.Character, INTERIOR_ID, ROOM_ONE_KEY);
        }
        void refreshGarageRoom()
        {
            Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, Game.Player.Character, INTERIOR_ID, ROOM_GARAGE_KEY);
        }
        void refreshVehiclesOnGarage()
        {
            var garagePosition = new Vector3(878.0764f, -1584.06f, 30.87137f);
            var distance = World.GetDistance(Game.Player.Character.Position, garagePosition);
            if (distance <= 10f)
            {
                Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, Game.Player.Character, INTERIOR_ID, ROOM_GARAGE_KEY);
                Vehicle[] vehicles = World.GetNearbyVehicles(garagePosition, 20f);
                foreach (var veh in vehicles)
                {
                    Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, veh, INTERIOR_ID, ROOM_GARAGE_KEY);
                }
            }
        }
        void setTeleportPoint()
        {
            POINT_POSITION = Game.Player.Character.Position;
            Notification.Show("Current Position ~g~Saved");
        }
        void teleportToPoint()
        {
            if (POINT_POSITION != new Vector3(0, 0, 0))
            {
                Function.Call(Hash.START_PLAYER_TELEPORT,
                    Game.Player,
                    POINT_POSITION.X, POINT_POSITION.Y, POINT_POSITION.Z,
                    false, true, true);
            }
        }
        void teleportToMlo()
        {
            if (HOUSE_POSITION != new Vector3(0, 0, 0))
            {
                Function.Call(Hash.START_PLAYER_TELEPORT, Game.Player, HOUSE_POSITION.X, HOUSE_POSITION.Y, HOUSE_POSITION.Z, false, true, true);
            }
        }
        #endregion

        #region config
        void setUpConfig()
        {
            Helper.SetConfigValue<int>(CONFIG_NAME, "INTERIOR", "INTERIOR_ID", 68610);
            Helper.SetConfigValue<uint>(CONFIG_NAME, "INTERIOR", "ROOM_ONE_KEY", 1757644675);
            Helper.SetConfigValue<uint>(CONFIG_NAME, "INTERIOR", "ROOM_GARAGE_KEY", 2570446951);

            Helper.SetConfigValue<float>(CONFIG_NAME, "INTERIOR", "HOUSE_POSITION_X", 870.3091f);
            Helper.SetConfigValue<float>(CONFIG_NAME, "INTERIOR", "HOUSE_POSITION_Y", -1586.659f);
            Helper.SetConfigValue<float>(CONFIG_NAME, "INTERIOR", "HOUSE_POSITION_Z", 31.60189f);
            loadConfig();
        }
        void loadConfig()
        {
            try
            {
                INTERIOR_ID = Helper.GetConfigValue(CONFIG_NAME, "INTERIOR", "INTERIOR_ID", 0);
                ROOM_ONE_KEY = Helper.GetConfigValue<uint>(CONFIG_NAME, "INTERIOR", "ROOM_ONE_KEY", 0);
                ROOM_GARAGE_KEY = Helper.GetConfigValue<uint>(CONFIG_NAME, "INTERIOR", "ROOM_GARAGE_KEY", 0);

                float housePositionX = Helper.GetConfigValue<float>(CONFIG_NAME, "INTERIOR", "HOUSE_POSITION_X", -0.000001955f);
                float housePositionY = Helper.GetConfigValue<float>(CONFIG_NAME, "INTERIOR", "HOUSE_POSITION_Y", -0.000001955f);
                float housePositionZ = Helper.GetConfigValue<float>(CONFIG_NAME, "INTERIOR", "HOUSE_POSITION_Z", -0.000001955f);

                if (housePositionX != -0.000001955f && housePositionY != 0.000001955f && housePositionZ != 0.000001955f)
                {
                    HOUSE_POSITION = new Vector3(housePositionX, housePositionY, housePositionZ);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
