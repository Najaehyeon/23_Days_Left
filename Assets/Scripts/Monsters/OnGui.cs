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
        public List<CreatureController> creatures = new List<CreatureController>();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }

        public void OnGUI()
        {
            if (!isDebug) return;

            GUIStyle buttonStyle = new(GUI.skin.button);
            buttonStyle.fontSize = 25;
            
            // hit
            if (GUILayout.Button("Hit", buttonStyle))
            {
                for (int i = 0; i < creatures.Count; i++)
                {
                    creatures[i].OnHit(20f);
                }
            }
        }
    }
}