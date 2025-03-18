using System.Collections.Generic;
using _23DaysLeft.Managers;
using _23DaysLeft.Monsters;
using UnityEngine;

namespace _23DaysLeft.Utils
{
    public class OnGui : MonoBehaviour
    {
        public bool isDebug = true;
        [SerializeField] private CreatureController creature;
        public void OnGUI()
        {
            if (!isDebug) return;

            GUIStyle buttonStyle = new(GUI.skin.button);
            buttonStyle.fontSize = 25;
        }
    }
}