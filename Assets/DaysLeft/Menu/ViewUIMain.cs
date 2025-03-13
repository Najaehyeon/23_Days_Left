namespace DaysLeft.Menu
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ViewUIMain : UIScreen
    {
        public void OnPressedSettingButton()
        {
            Controller.Show<ViewUISetting>();
        }
    }
}
