using GTA;
using GTA.Native;
using System.Windows.Forms;

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
                Function.Call(Hash.SET_INTERIOR_ACTIVE, 68354, true);
                Function.Call(Hash.DISABLE_INTERIOR, 68354, false);
                //Function.Call(Hash.SET_INTERIOR_ACTIVE, 68354, false);
                GTA.UI.Notification.Show("~g~Interior Loader Mod ~s~Loaded");
            }
        }
    }
}
