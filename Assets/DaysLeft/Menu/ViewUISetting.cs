namespace DaysLeft.Menu
{
    using System;
    using System.Collections.Generic;
#if FUSION_ENABLE_TEXTMESHPRO
  using Dropdown = TMPro.TMP_Dropdown;
  using InputField = TMPro.TMP_InputField;
  using Text = TMPro.TMP_Text;
#else
    using Dropdown = UnityEngine.UI.Dropdown;
    using InputField = UnityEngine.UI.InputField;
    using Text = UnityEngine.UI.Text;
#endif
    using UnityEngine;
    using UnityEngine.UI;

    public class ViewUISetting : UIScreen
    {
        public void OnBackButtonPressed()
        {
            Controller.Show<ViewUIMain>();
        }
    }
}
