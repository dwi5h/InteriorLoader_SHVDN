using GTA;
using GTA.Native;
using System.Windows.Forms;
using DieptidiUtility_SHVDN;
using System.IO;
using NativeUI;
using System.Collections.Generic;

namespace InteriorLoader_SHVDN
{
    public class Main : Script
    {
        MenuPool menuPool;
        UIMenu uIBaseMenu;
        List<UIMenuItem> menuItems;

        public Main()
        {
            menuItems = new List<UIMenuItem>();
            Tick += Main_Tick; ;
            KeyUp += Main_KeyUp;

            uIBaseMenu = new UIMenu("Interior Loader", "");
            uIBaseMenu.MouseControlsEnabled = true;
            uIBaseMenu.AddItem(new UIMenuItem("Enter Main Room"));
            uIBaseMenu.AddItem(new UIMenuItem("Enter Garage Room"));
            uIBaseMenu.AddItem(new UIMenuItem("Refresh Vehicles On Garage"));

            uIBaseMenu.OnItemSelect += UIBaseMenu_OnItemSelect;

            menuPool = new MenuPool();
            menuPool.Add(uIBaseMenu);
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
                    refreshVehiclesOnGarage();
                    break;
                default:
                    break;
            }
        }

        private void Main_Tick(object sender, System.EventArgs e)
        {
            menuPool.ProcessMenus();
        }

        private void Main_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                try
                {
                    uIBaseMenu.Visible = !uIBaseMenu.Visible;
                }
                catch (System.Exception ex)
                {
                    GTA.UI.Notification.Show("error: "+ex.Message);

                }
            }
        }

        void refreshRoomOne()
        {
            Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, Game.Player.Character, 68354, 1757644675);
        }
        void refreshGarageRoom()
        {
            Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, Game.Player.Character, 68354, 2570446951);
        }
        void refreshVehiclesOnGarage()
        {
            Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, Game.Player.Character, 68354, 2570446951);
            Vehicle[] vehicles = Helper.GetNearbyVehiclesInFrontPlayer(10f);
            foreach (var veh in vehicles)
            {
                Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, veh, 68354, 2570446951);
            }
        }
    }
}
