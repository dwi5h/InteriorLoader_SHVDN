using GTA;
using GTA.Native;
using System.Windows.Forms;
using System.IO;

namespace InteriorLoader_SHVDN
{
    public class Main : Script
    {
        public Main()
        {
            KeyUp += Main_KeyUp;
        }

        private void Main_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                try
                {
                    //uint key = Function.Call<uint>(Hash.GET_ROOM_KEY_FROM_ENTITY, Game.Player.Character);
                    Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, Game.Player.Character, 68354, 2570446951);
                    Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, Game.Player.Character.CurrentVehicle, 68354, 2570446951);
                    //File.WriteAllText(@"E:\PC\GTAV\log.txt", key.ToString());
                    //Function.Call(Hash.REFRESH_INTERIOR, 68354);
                    //Function.Call(Hash.SET_INTERIOR_ACTIVE, 68354, true);
                    //Function.Call(Hash.DISABLE_INTERIOR, 68354, false);
                    //Function.Call(Hash.SET_INTERIOR_ACTIVE, 68354, false);
                    GTA.UI.Notification.Show("~g~Interior Loader Mod ~s~Loaded ");
                }
                catch (System.Exception ex)
                {
                    GTA.UI.Notification.Show("error: "+ex.Message);

                }
            }
        }

        void getRoomKey()
        {
            //int = Function.Call(Hash.DISABLE_INTERIOR, 68354, false);
        }
    }
}
