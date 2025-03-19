using DaysLeft.Menu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildUIController : UIController
{
    public GameObject panel;

    public bool Toggle()
    {
        if (!panel.activeSelf)
        {
            panel.SetActive(true);
            return true;
        }
        else
        {
            panel.SetActive(false);
            Hide();
            return false;
        }
    }

}
